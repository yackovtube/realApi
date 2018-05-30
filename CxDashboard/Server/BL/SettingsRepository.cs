using DAL;
using DAL.Providers;
using Entities;
using Entities.Requests;
using Entities.Response;
using Entities.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class SettingsRepository
    {
        public IEnumerable<Category> GetCategoryList()
        {
            CategoriesDal categoriesDal = new CategoriesDal();
            return categoriesDal.GetAllReports().ToArray();
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

            var queriesData = await HttpProvider.GetHttpRequest("Enterprise/_apis/wit/queries/Shared%20Queries/Release%20Dashboard/Reports/" + category.CategoryName + "?$depth=1");
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

        //public async Task<IEnumerable<Report>> GetReportsNameByCategoryId(string categoryId)
        //{
        //    var responseData = new QueriesDataResponse();
        //    List<QueryData> Querieslst = new List<QueryData>();
        //    ReportsDal reportsDal = new ReportsDal();

        //    int categoryIdNum = 0;
        //    int.TryParse(categoryId, out categoryIdNum);
        //    return reportsDal.GetReportsByCategoryId(categoryIdNum);
        //}


        public async Task<IEnumerable<QueryDefinitionResponse>> GetQueryColumn(string queryId)
        {
            List<QueryDefinitionResponse> responselst = new List<QueryDefinitionResponse>();
            var queryData = await HttpProvider.GetHttpRequest("Enterprise/_apis/wit/queries/" + queryId + "?api-version=2.2&$expand=all");
            QueryDefinitionRequest queryDataObject = JsonConvert.DeserializeObject<QueryDefinitionRequest>(queryData);
            foreach (var item in queryDataObject.columns)
            {
                var columnData = new QueryDefinitionResponse() { Name = item.name, ReferenceName = item.referenceName };
                responselst.Add(columnData);
            }
            return responselst;
        }

        public async Task<dynamic> GetColumnType(string fieldName)
        {
            string columnType = await HttpProvider.GetHttpRequest("_apis/wit/fields/" + fieldName + "?api-version=1.0");
            dynamic typequery = JObject.Parse(columnType);

            return typequery;
        }
        public async Task<bool> AddReportChart(ReportChartModel report)
        {
            ReportChartDal dal = new ReportChartDal();
            return await dal.SaveChart(report);
        }
    }
}
