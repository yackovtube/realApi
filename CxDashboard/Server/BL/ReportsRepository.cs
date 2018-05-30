using DAL;
using Entities;
using Entities.Charts;
using Entities.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Entities.Responses;
using DAL.Providers;
using System;

namespace BL
{
    public class ReportsRepository
    {
        public IEnumerable<Category> GetCategoryList()
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            return categoriesDal.GetAllCategoriesIncludeReports();
        }

        public async Task<IEnumerable<Pie>> GetChartList(string reportName, string categoryName)
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            ReportChartDal chartsDal = new ReportChartDal();
            var ChartsList = new List<Pie>();
            Category category = categoriesDal.GetAllCategoriesIncludeReports().FirstOrDefault(r => r.CategoryName == categoryName);
            Report report = category.Reports.FirstOrDefault(r => r.ReportName == reportName);
            IEnumerable<ReportChartModel> reportsCarts = chartsDal.GetChartsList(r => r.ReportName == report.ReportId);

            foreach (var item in reportsCarts)
            {
                WorkItems workItemsData = await GetWorkItemData(item.QueryId);

                var ids = workItemsData.GetAllIds();
                WorkItemResponse response = null;
                foreach (var id in ids)
                {
                    var chartData = await HttpProvider.GetHttpRequest("_apis/wit/workItems?ids=" + id + "&fields=" + item.ColumnReferenceName);
                    var currentItem = JsonConvert.DeserializeObject<WorkItemResponse>(chartData);
                    if (response == null)
                    {
                        response = currentItem;
                    }
                    else
                    {
                        response.value.AddRange(currentItem.value);
                    }

                }

                var propName = item.ColumnReferenceName.Replace(".", "");

                var groupedData = response.value.GroupBy(p => p.fields.GetType().GetProperty(propName).GetValue(p.fields));

                List<Datum> PieLabels = new List<Datum>();
                int total = 0;
                foreach (var PieData in groupedData)
                {
                    total += PieData.Count();
                    PieLabels.Add(new Datum
                    {
                        Label = PieData.Key == null ? "No data" : PieData.Key.ToString(),
                        Value = PieData.Count()
                    });
                }

                var pie = new Pie
                {
                    Name = item.ChartName,
                    Element = "morris-donut-chart" + ChartsList.Count,
                    Type = "Donut",
                    Data = PieLabels.ToArray(),
                    TotalData = total.ToString(),
                    DataShowType = item.DataShowType.ToString().ToLower()
                };
                ChartsList.Add(pie);

            }

            return ChartsList;
        }
        public async Task<IEnumerable<InformationChart>> GetApplicationGrade()
        {
            List<InformationChart> infoChartLst = new List<InformationChart>();
            var queriesData = await HttpProvider.GetHttpRequest("Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/Application%20Grade?$depth=1");
            QueriesDataRequest queriesDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(queriesData);
            List<QueryData> Querieslst = new List<QueryData>();

            foreach (var item in queriesDataObject.children)
            {
                var queryData = new QueryData() { Id = item.id, Name = item.name };
                queryData.workItems = await GetWorkItemData(queryData.Id);
                Querieslst.Add(queryData);
            }
            int critical = Querieslst.FirstOrDefault(q => q.Name.Contains("Critical #")).workItems.WorkItemCount;
            int high = Querieslst.FirstOrDefault(q => q.Name.Contains("High #")).workItems.WorkItemCount;
            int medium = Querieslst.FirstOrDefault(q => q.Name.Contains("Medium #")).workItems.WorkItemCount;

            double totalCalc = 100 - (5 * critical) - (0.5 * high) - (0.1 * medium);
            InformationChart infoChart = new InformationChart
            {
                name = "Application server grade",
                Data = totalCalc.ToString(),
                ChartData = new List<InformationChartData> {
                    new InformationChartData { Data = critical.ToString(), DataName = "Critical", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Critical #"))._links.html.href },
                    new InformationChartData { Data = high.ToString(), DataName = "High", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("High #"))._links.html.href },
                    new InformationChartData { Data = medium.ToString(), DataName = "Medium", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Medium #"))._links.html.href }
                }
            };
            if (totalCalc >= 70)
            {
                infoChart.color = "green";
                infoChart.Marx = "check";
            }
            else if (totalCalc >= 60)
            {
                infoChart.color = "yellow";
                infoChart.Marx = "exclamation";
            }
            else
            {
                infoChart.color = "red";
                infoChart.Marx = "times";
            }

            infoChartLst.Add(infoChart);

            return infoChartLst;
        }

        private static async Task<WorkItems> GetWorkItemData(string queryId)
        {
            string data = await HttpProvider.GetHttpRequest("Enterprise/_apis/wit/queries/" + queryId + "?api-version=2.2&$expand=all");
            QueryDefinitionRequest queryData = JsonConvert.DeserializeObject<QueryDefinitionRequest>(data);
            var wiqlClass = new QueryWiql { query = queryData.wiql };
            string query = JsonConvert.SerializeObject(wiqlClass, Formatting.None); //@"{ ""query"": """ + queryData.wiql + @"""}";
            string workItemsString = await HttpProvider.PostHttpRequest("Enterprise/_apis/wit/wiql?api-version=2.2", query);
            var workItems = (JObject)JsonConvert.DeserializeObject(workItemsString);
            string queryType = workItems["queryType"].Value<string>();

            WorkItems workItemsData = default(WorkItems);

            switch (queryType)
            {
                case "flat":
                    workItemsData = JsonConvert.DeserializeObject<Flat>(workItemsString);
                    break;
                case "OneHop":
                    workItemsData = JsonConvert.DeserializeObject<OneHop>(workItemsString);
                    break;

                case default(string):
                    break;
            }

            return workItemsData;
        }

    }
    [Serializable]
    public class QueryWiql
    {
        public string query { get; set; }
    }
}
