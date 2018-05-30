using CxDashboard.DAL;
using CxDashboard.DAL.Providers;
using CxDashboard.Entities;
using CxDashboard.Entities.Charts;
using CxDashboard.Entities.Requests;
using CxDashboard.Entities.Responses;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CxDashboard.BL
{
    public class ReportsRepository
    {
        public IEnumerable<Category> GetCategoryList()
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            return categoriesDal.GetAllCategoriesIncludeReports();
        }

        public ReportChartModelResponse GetChartByName(string categoryName, string reportName, string chartName)
        {
            ReportChartModel currentChart = GetCart(categoryName, reportName, chartName);
            var reportChrtData = new ReportChartModelResponse()
            {
                ReportChartId = currentChart.ReportChartId,
                DataShowType = currentChart.DataShowType.ToString().ToLower(),
                ChartType = currentChart.ChartType.ToString().ToLower(),
                aggregationColumnReferenceName = currentChart.aggregationColumnReferenceName,
                CategoryId = currentChart.CategoryId.ToString(),
                ChartName = currentChart.ChartName,
                ColumnReferenceName = currentChart.ColumnReferenceName,
                QueryId = currentChart.QueryId,
                ReportName = currentChart.ReportName.ToString()
            };
            return reportChrtData;
        }

        private static ReportChartModel GetCart(string categoryName, string reportName, string chartName)
        {
            ReportChartModel currentChart = null;
            CategoriesDal categoryDal = new CategoriesDal();
            var categoryId = categoryDal.GetCategoryIdByName(categoryName);
            if (categoryId > 0)
            {
                ReportsDal reportDal = new ReportsDal();
                var reportId = reportDal.GetReportIdByNameAndCategoryId(categoryId, reportName);

                if (reportId > 0)
                {
                    ReportChartDal reportChart = new ReportChartDal();
                    currentChart = reportChart.GetChartByNameAndCartegoryReportIds(categoryId, reportId, chartName);
                }
            }
            return currentChart;
        }

        public async Task<IEnumerable<Chart>> GetChartList(string reportName, string categoryName)
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            ReportChartDal chartsDal = new ReportChartDal();
            var ChartsList = new List<Chart>();
            Category category = categoriesDal.GetAllCategoriesIncludeReports().FirstOrDefault(r => r.CategoryName == categoryName);
            Report report = category.Reports.FirstOrDefault(r => r.ReportName == reportName);
            IEnumerable<ReportChartModel> reportsCarts = chartsDal.GetChartsList(r => r.ReportName == report.ReportId);

            foreach (var item in reportsCarts)
            {
                try
                {
                    switch (item.ChartType)
                    {
                        case Entities.Enums.ChartType.Pie:
                            Pie pie = await GeneratPie(item, ChartsList.Count);
                            ChartsList.Add(pie);
                            break;
                        case Entities.Enums.ChartType.Line:
                            Linear linear = await GeneratLinear(item, ChartsList.Count);
                            ChartsList.Add(linear);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }

            }

            return ChartsList;
        }

        private static async Task<Linear> GeneratLinear(ReportChartModel item, int position)
        {
            var linear = new Linear() { Type = item.ChartType.ToString().ToLower(), Element = "linear" + position };

            return linear;
        }

        private static async Task<Pie> GeneratPie(ReportChartModel item, int position)
        {
            var workItemData = await GetWorkItemData(item.QueryId, null);
            WorkItems workItemsData = workItemData.Item1;

            var ids = workItemsData.GetAllIds();
            WorkItemResponse response = null;
            bool aggregation = false;
            foreach (var id in ids)
            {
                string aggregationFiled = "";
                if (!string.IsNullOrEmpty(item.aggregationColumnReferenceName))
                {
                    aggregationFiled = "," + item.aggregationColumnReferenceName;
                    aggregation = true;
                }
                var chartData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "_apis/wit/workItems?ids=" + id + "&fields=" + item.ColumnReferenceName + aggregationFiled);
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

            var groupedData = response.value.GroupBy(p => p.fields.GetType().GetProperty(propName).GetValue(p.fields)).Select(g => new
            {
                titel = g.Key,
                value = (double)g.Count()
            });

            if (aggregation)
            {
                var aggregationPropName = item.aggregationColumnReferenceName.Replace(".", "");
                groupedData = response.value.GroupBy(p => p.fields.GetType().GetProperty(propName).GetValue(p.fields)).Select(g => new
                {
                    titel = g.Key,
                    value = g.Sum(f => (double)f.fields.GetType().GetProperty(aggregationPropName).GetValue(f.fields))
                });
            }

            List<Datum> PieLabels = new List<Datum>();
            int total = response.value.Count;
            foreach (var PieData in groupedData)
            {

                PieLabels.Add(new Datum
                {
                    Label = PieData.titel == null ? "No data" : PieData.titel.ToString(),
                    Value = PieData.value,
                });
            }

            var pie = new Pie
            {
                Name = item.ChartName,
                Element = "pie-chart" + position,
                Type = item.ChartType.ToString().ToLower(),
                Data = PieLabels.ToArray(),
                TotalData = total.ToString(),
                DataShowType = item.DataShowType.ToString().ToLower()
            };
            return pie;
        }

        public async Task<string> GetFilterQueryLink(string categoryName, string reportName, string chartName, string filterBy)
        {
            string QueryUrl = "";
            var queriesData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/Temp%20Queries?$depth=1");
            QueriesDataRequest queriesDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(queriesData);
            if (queriesDataObject.children == null || queriesDataObject.children.Count() <= 50)
            {
                var chartData = GetCart(categoryName, reportName, chartName);
                if (chartData != null)
                {
                    var wiql = await GetQueryWiql(chartData.QueryId);
                    var newWiql = wiql.Replace("where", string.Format(@"where [{0}] = ""{1}"" and", chartData.ColumnReferenceName, filterBy));
                    var uniqueKey = string.Format("{0} - {1}", chartData.ChartName, filterBy);
                    QueryUrl = await GeneratTempQuery(newWiql, uniqueKey);
                }
            }
            return QueryUrl;
        }

        public async Task<string> GeneratTempQuery(string newWiql, string queryName)
        {
            string QueryUrl = "";
            var uniqueKey = string.Format("{0} ({1})", queryName, Guid.NewGuid().ToString("N").Substring(0, 4));
            newWiql = newWiql.Replace("%3D", "=");
            var newQueryData = new NewQueryClass() { Name = uniqueKey, Wiql = newWiql };
            string query = JsonConvert.SerializeObject(newQueryData, Formatting.None);
            var responseUrl = await HttpProvider.PostHttpRequest("Enterprise/_apis/wit/queries/Shared%20Queries%2FRelease%20Dashboard%2FTemp%20Queries?api-version=2.2", query);
            QueriesDataRequest queryDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(responseUrl);
            QueryUrl = queryDataObject._links.html.href;

            Task.Run(() => RemoveTempQueryFromTfs(queryDataObject.id));
            return QueryUrl;
        }

        public async void RemoveTempQueryFromTfs(string queryId)
        {
            await Task.Delay(60000);
            var response = await HttpProvider.DeleteHttpRequest("Enterprise/_apis/wit/queries/" + queryId + "?api-version=2.2");
        }

        public async Task<bool> RemoveChart(int id)
        {
            ReportChartDal reportDal = new ReportChartDal();
            return await reportDal.DeleteChart(id);
        }

        public void UpdateChart(int id, ReportChartModel report)
        {
            ReportChartDal reportChart = new ReportChartDal();
            reportChart.UpdateChart(id, report);
        }

        public async Task<InformationChart> GetApplicationGrade()
        {
            var applicationGradeResult = await getGradeValues("Application%20Grade");

            double totalCalc = 100 - (5 * applicationGradeResult.Item1) - (0.5 * applicationGradeResult.Item2) - (0.1 * applicationGradeResult.Item3);
            InformationChart infoChart = new InformationChart
            {
                Name = "Core Application quality grade",
                Data = totalCalc.ToString(),
                ChartData = applicationGradeResult.Item4
            };
            if (applicationGradeResult.Item1 > 0 || totalCalc < 60)
            {
                infoChart.Color = "red";
                infoChart.Marx = "times";
            }
            else if (totalCalc < 82.3)
            {
                infoChart.Color = "yellow";
                infoChart.Marx = "exclamation";
            }
            else
            {
                infoChart.Color = "green";
                infoChart.Marx = "check";
            }

            return infoChart;
        }

        public async Task<OlderInformationChart> GetEngineGrade(string versin)
        {
            OlderInformationChart oldVersionChart = new OlderInformationChart();

            try
            {
                var engineApiResult = await HttpProvider.GetHttpRequest(StaticParams.EngineAPIUrl, "?version=" + versin);
                var engineAPIObject = JsonConvert.DeserializeObject<EngineAPIData>(engineApiResult);

                double newGoodHiddenBad = engineAPIObject.newGood + engineAPIObject.hiddenBad;
                double NewHiddenGoodBad = engineAPIObject.newBad + engineAPIObject.newGood + engineAPIObject.hiddenBad + engineAPIObject.hiddenGood;

                double newGoodBad = engineAPIObject.newBad + engineAPIObject.newGood;
                double hiddenGoodBad = engineAPIObject.hiddenBad + engineAPIObject.hiddenGood;
                double unanalyzed = engineAPIObject.hiddenUnknown + engineAPIObject.newUnknown;
                double totalAnalyzed = engineAPIObject.hiddenGood + engineAPIObject.hiddenBad + engineAPIObject.newGood + engineAPIObject.newBad + unanalyzed;

                double Analyzed = totalAnalyzed - unanalyzed;
                double precentOfAnalyzed = Analyzed == 0 || totalAnalyzed == 0 ? 0 : (Analyzed / totalAnalyzed) * 100;
                double precentOfRemovedBad = engineAPIObject.hiddenBad == 0 || hiddenGoodBad == 0 ? 0 : (engineAPIObject.hiddenBad / hiddenGoodBad) * 100;
                double precentOfNewGood = engineAPIObject.newGood == 0 || newGoodBad == 0 ? 0 : (engineAPIObject.newGood / newGoodBad) * 100;
                double engineTotalGrade = newGoodHiddenBad == 0 || NewHiddenGoodBad == 0 ? 0 : (newGoodHiddenBad / NewHiddenGoodBad) * 100;

                oldVersionChart.ApplicationVersion = versin;
                oldVersionChart.Name = "Engine quality grade";
                oldVersionChart.Data = string.Format("{0:0.00}", engineTotalGrade);

                if (engineTotalGrade >= 95 && precentOfAnalyzed >= 80)
                {
                    oldVersionChart.Color = "green";
                    oldVersionChart.Marx = "check";
                }
                else if (engineTotalGrade >= 70)
                {
                    oldVersionChart.Color = "yellow";
                    oldVersionChart.Marx = "exclamation";
                }
                else
                {
                    oldVersionChart.Color = "red";
                    oldVersionChart.Marx = "times";
                }

                oldVersionChart.ChartData = new List<InformationChartData>
                    {
                        new InformationChartData { Data = string.Format("{0:0.0}", precentOfNewGood), DataName = "New good %" },
                        new InformationChartData { Data = string.Format("{0:0.0}", precentOfRemovedBad), DataName = "Removed bad %" },
                        new InformationChartData { Data = string.Format("{0:0.0}", precentOfAnalyzed), DataName = "Analyzed %" },
                        new InformationChartData { Data = "", DataName = "" },
                        new InformationChartData { Data = string.Format("{0:0,0}", unanalyzed), DataName = "Unanalyzed count" },
                        new InformationChartData { Data = string.Format("{0:0,0}", Analyzed), DataName = "Analyzed count" },
                        new InformationChartData { Data = string.Format("{0:0,0}", totalAnalyzed), DataName = "Total count" }
                    };
                return oldVersionChart;
            }
            catch (Exception ex)
            {
                oldVersionChart.ApplicationVersion = versin;
                oldVersionChart.Data = "NA";
                oldVersionChart.Color = "red";
                oldVersionChart.Marx = "question";
                oldVersionChart.Name = "Engine quality grade";


                return oldVersionChart;
            }


        }

        public IEnumerable<BaseInformationChart> GetSmokeGrades()
        {
            List<OlderInformationChart> smokeList = new List<OlderInformationChart>();

            try
            {
                var node = new Uri(StaticParams.SmokeApiUrl);
                var settings = new ConnectionSettings(node);
                var client = new ElasticClient(settings);

                const string byStage = "by_stage";
                const string byStatus = "by_status";
                var searchResults = client.Search<SmokeEntityRequest>(s => s
                    .Index("cx-metrics")
                    .Type("8_6")
                    .Query(q => q.Term(f => f.Pipeline, "smoke") &&
                                q.Term(f => f.State, "end") &&
                                q.DateRange(r => r.Name("time")
                                                  .Field(f => f.Timestamp)
                                                  .LessThan(DateTime.Now)
                                                  .GreaterThan(DateTime.Now.AddDays(-30)))

                    )
                    .Aggregations(a => a
                            .Terms(byStage, t => t
                                .Field(f => f.Stage)
                                .Size(3)
                                .Order(TermsOrder.CountDescending)
                                .Aggregations(t1 => t1
                                    .Terms(byStatus, aa => aa
                                        .Field(f => f.Status)
                                        .Size(2)
                                        .Order(TermsOrder.CountDescending)
                                )
                            )
                        )
                    )
                );


                var buckets = searchResults.Aggs.Terms(byStage).Buckets
                           .ToDictionary(x => x.Key, x =>
                           {
                               var aggregation = (BucketAggregate)x.Aggregations[byStatus];
                               return aggregation.Items.Cast<KeyedBucket<object>>()
                                                 .ToDictionary(y => y.Key.ToString().ToLower(),
                                                               y => y.DocCount);
                           });

                foreach (var bucket in buckets)
                {
                    var SmokeItem = new OlderInformationChart();
                    SmokeItem.Name = string.Format("{0} smoke success rate (Last 30 days)", bucket.Key.First().ToString().ToUpper() + bucket.Key.Substring(1));
                    SmokeItem.Color = "default";

                    long? total = 0;
                    long? success = 0;
                    foreach (var item in bucket.Value)
                    {
                        total += item.Value;
                        if (item.Key == "success")
                            success = item.Value;
                    }

                    double precentage = ((double)success / (double)total) * 100;
                    int fail = total > success ? (int)(total - success) : 0;
                    SmokeItem.Data = string.Format("{0:0.##}%", precentage);
                    //SmokeItem.Marx = "question";
                    SmokeItem.ChartData = new List<InformationChartData>
                    {
                        new InformationChartData { Data = success.ToString(), DataName = "Success #" },
                        new InformationChartData { Data = fail.ToString(), DataName = "Fail #" },
                        new InformationChartData { Data = "", DataName = "" },
                        new InformationChartData { Data = total.ToString(), DataName = "Total #" }
                    };


                    smokeList.Add(SmokeItem);
                }
            }
            catch (Exception ex)
            {

                smokeList.Add(new OlderInformationChart { Color = "red", Data = "NA", Marx = "question", Name = "Smoke Grade" });
            }

            smokeList = smokeList.OrderByDescending(k => k.Name).ToList();

            return smokeList;
        }

        private async Task<IEnumerable<BaseInformationChart>> GetCodeCoverageGrade(string buildDefinition)
        {
            var infoChartList = new List<BaseInformationChart>();
            try
            {
                string url = string.Format("Enterprise/_apis/build/builds?definition={0}&api-version=1.0&$top={1}", buildDefinition, 10);
                var result = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, url);
                var buildsLst = JsonConvert.DeserializeObject<BuildRequest>(result);

                foreach (var item in buildsLst.value)
                {
                    string codeCoverageUrl = string.Format("Enterprise/_apis/test/codeCoverage?buildId={0}&api-version=2.0-preview", item.id);
                    var codeCoverageResult = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, codeCoverageUrl);
                    var codeCoverage = JsonConvert.DeserializeObject<CodeCoverageRequest>(codeCoverageResult);

                    if (codeCoverage.coverageData != null && codeCoverage.coverageData.Length > 0)
                    {
                        var coverageData = codeCoverage.coverageData[0];
                        if (coverageData.coverageStats != null && coverageData.coverageStats.Length > 0)
                        {
                            var stats = coverageData.coverageStats;
                            foreach (var stat in stats)
                            {
                                var coveragePrecentage = ((double)stat.covered / (double)stat.total) * 100;
                                var chart = new BaseInformationChart
                                {
                                    Name = string.Format("Code {0} UT coverage", stat.label.ToLower()),
                                    Data = string.Format("{0:0.##}%", coveragePrecentage),
                                    Color = "default"
                                };
                                infoChartList.Add(chart);
                            }
                            break;
                        }
                    }
                }
                if (infoChartList.Count == 0)
                {
                    infoChartList.Add(new BaseInformationChart { Color = "default", Data = "NA", Name = "Code coverage not exist" });
                    infoChartList.Add(new BaseInformationChart { Color = "default", Data = "NA", Name = "Code coverage not exist" });
                }
            }
            catch (Exception)
            {
                infoChartList.Add(new BaseInformationChart { Color = "red", Data = "NA", Marx = "question", Name = "Code coverage grade" });
            }
            return infoChartList;
        }

        private async Task<GradeAndInformationData> Rc1ActualGrade(double assumedGrade, string applicationVersion, DateTimeOffset versionSetDate, DateTimeOffset versionTopDate)
        {
            var gradeResult = new GradeAndInformationData();
            try
            {
                var targetVersion = new Tuple<string, string, string>("Cx.TargetVersion", "=", applicationVersion);
                var StartdateVersion = new Tuple<string, string, string>("System.CreatedDate", ">=", versionSetDate.ToString("yyyy-MM-dd'T'00:00:00.0000000"));
                Tuple<string, string, string> EndDateVersion = null;
                if (versionTopDate != DateTimeOffset.MinValue)
                {
                    EndDateVersion = new Tuple<string, string, string>("System.CreatedDate", "<=", versionTopDate.AddDays(-1).ToString("yyyy-MM-dd'T'00:00:00.0000000"));
                }
                var parms = new List<Tuple<string, string, string>>() { targetVersion, StartdateVersion, EndDateVersion };
                var gradesList = await getGradeValuesByFilters("Application%20prev%20releases%20grades/RC1%20actual", parms);
                gradeResult.Grade = string.Format("{0:0.#}", (assumedGrade - (5 * gradesList.Item1) - (0.5 * gradesList.Item2) - (0.1 * gradesList.Item3)));
                gradeResult.DataList = gradesList.Item4;
            }
            catch (Exception ex)
            {
                gradeResult.Grade = "NA";

            }

            return gradeResult;
        }

        private async Task<GradeAndInformationData> GaActualGrade(double assumedGrade, string applicationVersion, DateTimeOffset versionSetDate)
        {
            var gradeResult = new GradeAndInformationData();
            try
            {
                var targetVersion = new Tuple<string, string, string>("Cx.TargetVersion", "=", applicationVersion);
                var DateVersion = new Tuple<string, string, string>("System.CreatedDate", ">=", versionSetDate.ToString("yyyy-MM-dd'T'00:00:00.0000000"));
                var parms = new List<Tuple<string, string, string>>() { targetVersion, DateVersion };
                var gradesList = await getGradeValuesByFilters("Application%20prev%20releases%20grades/GA%20assumed", parms);
                gradeResult.Grade = string.Format("{0:0.#}", (assumedGrade - (5 * gradesList.Item1) - (0.5 * gradesList.Item2) - (0.1 * gradesList.Item3)));
                gradeResult.DataList = gradesList.Item4;
            }
            catch (Exception ex)
            {
                gradeResult.Grade = "NA";

            }

            return gradeResult;
        }

        public async Task<MainDashboardResponse> GetMainDashboardData()
        {
            MainDashboardResponse mainDashboard = new MainDashboardResponse();
            MainDashboardDal MainDashDal = new MainDashboardDal();
            OlderVersionDal oldVersionDal = new OlderVersionDal();

            byte[] imageBytes = MainDashDal.GetTimeLine();
            mainDashboard.TimeLine = imageBytes != null ? "data:image/png;base64," + Convert.ToBase64String(imageBytes, Base64FormattingOptions.None) : null;

            var oldVersionList = MainDashDal.GetLastApplicationVersions();
            List<ApplicationOlderVersionGradeResponse> appOldVerList = new List<ApplicationOlderVersionGradeResponse>();

            foreach (var item in oldVersionList)
            {
                var topDateTime = item.GAQuelityGrade > 0 && item.GADateTime != DateTimeOffset.MinValue ? item.GADateTime : DateTimeOffset.MinValue;
                var oldVersionItem = new ApplicationOlderVersionGradeResponse
                {
                    ApplicationVersion = item.ApplicationVersion,
                    GAQualityGrade = item.GAQuelityGrade,
                    GADateTime = item.GADateTime.ToString("dd/MM/yy") != "01/01/00" ? item.GADateTime.ToString("dd/MM/yy") : "",
                    RC1QualityGrade = item.RC1QuelityGrade,
                    RC1DateTime = item.RC1DateTime.ToString("dd/MM/yy") != "01/01/00" ? item.RC1DateTime.ToString("dd/MM/yy") : "",
                    RC1ActualGrade = await Rc1ActualGrade(item.RC1QuelityGrade, item.ApplicationVersion, item.RC1DateTime, topDateTime)
                };
                if (item.GAQuelityGrade > 0)
                {
                    oldVersionItem.GAActualGrade = await GaActualGrade(item.GAQuelityGrade, item.ApplicationVersion, item.GADateTime);
                }
                appOldVerList.Add(oldVersionItem);
            }



            mainDashboard.OldVersionApplicationList = appOldVerList;
            mainDashboard.CurrentApplicationKPIsGrade = await GetApplicationGrade();
            var currentVersionNumber = oldVersionDal.GetCurrentVersion();
            mainDashboard.CurrentEngineKPIsGrade = await GetEngineGrade(currentVersionNumber.EngineVersionNumber);

            List<OlderInformationChart> oldEngineGrades = new List<OlderInformationChart>();

            var oldEngineVersionList = oldVersionDal.GetLastOldEngineVersion();

            foreach (var item in oldEngineVersionList)
            {
                oldEngineGrades.Add(await GetEngineGrade(item.EngineVersionNumber));
            }

            mainDashboard.OldVersionEngineList = oldEngineGrades;

            mainDashboard.SmokeGrades = GetSmokeGrades();

            mainDashboard.EngineCodeCoverage = await GetCodeCoverageGrade(StaticParams.EngineBuildDefinition);
            mainDashboard.ApplicationCodeCoverage = await GetCodeCoverageGrade(StaticParams.AppBuildDefinition);

            return mainDashboard;
        }

        internal static async Task<Tuple<int, int, int, List<InformationChartData>>> getGradeValues(string GradeFolder)
        {
            var queriesData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/" + GradeFolder + "?$depth=1");
            QueriesDataRequest queriesDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(queriesData);
            List<QueryData> Querieslst = new List<QueryData>();

            foreach (var item in queriesDataObject.children)
            {
                var queryData = new QueryData() { Id = item.id, Name = item.name };
                var workItemData = await GetWorkItemData(queryData.Id, null);
                queryData.workItems = workItemData.Item1;
                Querieslst.Add(queryData);
            }
            int critical = Querieslst.FirstOrDefault(q => q.Name.Contains("Critical #")).workItems.WorkItemCount;
            int high = Querieslst.FirstOrDefault(q => q.Name.Contains("High #")).workItems.WorkItemCount;
            int medium = Querieslst.FirstOrDefault(q => q.Name.Contains("Medium #")).workItems.WorkItemCount;

            var infoData = new List<InformationChartData> {
                    new InformationChartData { Data = critical.ToString(), DataName = "Critical", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Critical #"))._links.html.href },
                    new InformationChartData { Data = high.ToString(), DataName = "High", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("High #"))._links.html.href },
                    new InformationChartData { Data = medium.ToString(), DataName = "Medium", Link = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Medium #"))._links.html.href }
                };

            return new Tuple<int, int, int, List<InformationChartData>>(critical, high, medium, infoData);
        }

        internal static async Task<Tuple<int, int, int, List<InformationChartData>>> getGradeValuesByFilters(string GradeFolder, List<Tuple<string, string, string>> filters)
        {
            var queriesData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/" + GradeFolder + "?$depth=1");
            QueriesDataRequest queriesDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(queriesData);
            List<QueryData> Querieslst = new List<QueryData>();

            foreach (var item in queriesDataObject.children)
            {
                var queryData = new QueryData() { Id = item.id, Name = item.name };
                var workItemData = await GetWorkItemData(queryData.Id, filters);
                queryData.workItems = workItemData.Item1;
                var wiql = workItemData.Item2.Replace("=", "%3D");
                item._links.html.href = wiql;
                Querieslst.Add(queryData);
            }
            int critical = Querieslst.FirstOrDefault(q => q.Name.Contains("Critical #")).workItems.WorkItemCount;
            int high = Querieslst.FirstOrDefault(q => q.Name.Contains("High #")).workItems.WorkItemCount;
            int medium = Querieslst.FirstOrDefault(q => q.Name.Contains("Medium #")).workItems.WorkItemCount;

            var infoData = new List<InformationChartData> {
                    new InformationChartData { Data = critical.ToString(), DataName = "Critical", Wiql = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Critical #"))._links.html.href },
                    new InformationChartData { Data = high.ToString(), DataName = "High", Wiql = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("High #"))._links.html.href },
                    new InformationChartData { Data = medium.ToString(), DataName = "Medium", Wiql = queriesDataObject.children.FirstOrDefault(q => q.name.Contains("Medium #"))._links.html.href }
                };

            return new Tuple<int, int, int, List<InformationChartData>>(critical, high, medium, infoData);
        }

        internal static async Task<Tuple<WorkItems, string>> GetWorkItemData(string queryId, List<Tuple<string, string, string>> filters)
        {
            string query = await GetQueryWiql(queryId);

            if (filters != null)
            {
                foreach (var item in filters)
                {
                    if (item != null && !string.IsNullOrEmpty(item.Item1) && !string.IsNullOrEmpty(item.Item1))
                    {
                        query = query.Replace("where", string.Format(@"where [{0}] {1} ""{2}"" and", item.Item1, item.Item2, item.Item3));
                    }
                }
            }

            var wiqlClass = new QueryWiql { query = query };
            string queryClass = JsonConvert.SerializeObject(wiqlClass, Formatting.None);
            string workItemsString = await HttpProvider.PostHttpRequest("Enterprise/_apis/wit/wiql?api-version=2.2", queryClass);
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

            return new Tuple<WorkItems, string>(workItemsData, query);
        }

        private static async Task<string> GetQueryWiql(string queryId)
        {
            string data = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/" + queryId + "?api-version=2.2&$expand=all");
            QueryDefinitionRequest queryData = JsonConvert.DeserializeObject<QueryDefinitionRequest>(data);

            return queryData.wiql;
        }
    }
    [Serializable]
    public class QueryWiql
    {
        public string query { get; set; }
    }
    [Serializable]
    public class NewQueryClass
    {
        public string Name { get; set; }
        public string Wiql { get; set; }
    }
}
