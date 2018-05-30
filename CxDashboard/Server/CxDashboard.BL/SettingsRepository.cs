using CxDashboard.DAL;
using CxDashboard.DAL.Providers;
using CxDashboard.Entities;
using CxDashboard.Entities.Requests;
using CxDashboard.Entities.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CxDashboard.BL
{
    public class SettingsRepository
    {
        public IEnumerable<Category> GetCategoryList()
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            return categoriesDal.GetAllCategories().ToArray();
        }

        public async Task<QueriesDataResponse> GetQueriesNReportsNameList(string categoryId)
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            ReportsDal reportsDal = new ReportsDal();
            var responseData = new QueriesDataResponse();
            List<QueryData> Querieslst = new List<QueryData>();

            int categoryIdNum = 0;
            int.TryParse(categoryId, out categoryIdNum);

            var category = categoriesDal.GetCategoryById(categoryIdNum);

            var queriesData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/Reports/" + category.CategoryName + "?$depth=1");
            QueriesDataRequest queriesDataObject = JsonConvert.DeserializeObject<QueriesDataRequest>(queriesData);
            foreach (var item in queriesDataObject.children)
            {
                var queryData = new QueryData() { Id = item.id, Name = item.name };
                Querieslst.Add(queryData);
            }
            responseData.AllAvailableQueries = Querieslst.ToArray();
            responseData.AllReports = reportsDal.GetReportsByCategoryId(categoryIdNum).ToArray();
            return responseData;
        }

        public async Task<QueryFieldsResponse> GetQueryColumn(string queryId)
        {
            QueryFieldsResponse response = new QueryFieldsResponse();
            response.GroupByFields = new List<QueryDefinitionResponse>();
            response.AggregationFields = new List<QueryDefinitionResponse>();
            var queryData = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "Enterprise/_apis/wit/queries/" + queryId + "?api-version=2.2&$expand=all");
            QueryDefinitionRequest queryDataObject = JsonConvert.DeserializeObject<QueryDefinitionRequest>(queryData);
            foreach (var item in queryDataObject.columns)
            {
                var columnData = new QueryDefinitionResponse() { Name = item.name, ReferenceName = item.referenceName };
                if (columnData.Name != "ID")
                {
                    response.GroupByFields.Add(columnData);
                    var columnType = await GetColumnType(item.referenceName);
                    if (columnType.type == "integer" || columnType.type == "double")
                    {
                        response.AggregationFields.Add(columnData);
                    } 
                }
            }
            return response;
        }

        public async Task<dynamic> GetColumnType(string fieldName)
        {
            string columnType = await HttpProvider.GetHttpRequest(StaticParams.TfsUrl, "_apis/wit/fields/" + fieldName + "?api-version=1.0");
            dynamic typequery = JObject.Parse(columnType);

            return typequery;
        }
        public async Task<bool> AddReportChart(ReportChartModel report)
        {
            ReportChartDal dal = new ReportChartDal();
            return await dal.SaveChart(report);
        }

        public void UpdateTimeline(string timeLine)
        {
            MainDashboardDal DashboardDal = new MainDashboardDal();

            var imageBase64 = Regex.Replace(timeLine, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            byte[] imageByte = Convert.FromBase64String(imageBase64);

            DashboardDal.SaveTimeLine(imageByte);
        }
        public async Task<ApplicationOlderVersionGrade> UpdateRC1GradeAndVersion(string version)
        {
            OlderVersionDal AppOldVersionDal = new OlderVersionDal();
            Version oldVersionToSave = new Version();

            var gradeValues = await ReportsRepository.getGradeValues("Application%20Grade");

            double totalCalc = 100 - (5 * gradeValues.Item1) - (0.5 * gradeValues.Item2) - (0.1 * gradeValues.Item3);

            if (Version.TryParse(version, out oldVersionToSave))
            {
                return AppOldVersionDal.SetApplictionOldVersionRC1(oldVersionToSave.ToString(), totalCalc);
            }

            return default(ApplicationOlderVersionGrade);
        }

        public async Task<ApplicationOlderVersionGrade> UpdateGAGrade()
        {
            OlderVersionDal AppOldVersionDal = new OlderVersionDal();
            var lastVersion = AppOldVersionDal.GetLastApplicationOldGrade();
            var targetVersion = new Tuple<string, string, string>("Cx.TargetVersion", "=", lastVersion.ApplicationVersion);
            var DateVersion = new Tuple<string, string, string>("System.CreatedDate", ">=", lastVersion.RC1DateTime.ToString("yyyy-MM-dd'T'00:00:00.0000000"));
            var parms = new List<Tuple<string, string, string>>() { targetVersion, DateVersion };

            var gradeValues = await ReportsRepository.getGradeValuesByFilters("Application%20prev%20releases%20grades/GA%20assumed", parms);

            double totalCalc = lastVersion.RC1QuelityGrade - (5 * gradeValues.Item1) - (0.5 * gradeValues.Item2) - (0.1 * gradeValues.Item3);

            return AppOldVersionDal.SetApplicationGaGrade(totalCalc);
        }

        public EngineVersion UpdateEngineVersion(string version)
        {
            OlderVersionDal oldVersionDal = new OlderVersionDal();
            Version oldVersionToSave = new Version();

            if (Version.TryParse(version, out oldVersionToSave) && (version.IndexOf('.') != version.LastIndexOf('.')))
            {
                return oldVersionDal.SetEngineOldVersion(version);
            }
            return default(EngineVersion);

        }
        
    }
}
