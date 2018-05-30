using CxDashboard.BL;
using CxDashboard.Entities;
using CxDashboard.Entities.Charts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CxDashboard.server.Controllers
{
    public class ReportsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(ReportChartModel))]
        public IHttpActionResult GetChartData(string categoryName, string reportName, string chartName)
        {
            ReportsRepository reportRepo = new ReportsRepository();
            var reportChart = reportRepo.GetChartByName(categoryName, reportName, chartName);

            if (reportChart == null)
            {
                return NotFound();
            }

            return Ok(reportChart);
        }
        [HttpGet]
        public IHttpActionResult GetCategoryReportsList()
        {
            try
            {
                ReportsRepository reportsRepo = new ReportsRepository();
                var item = new baseResponse<IEnumerable<Category>>() { responseObject = reportsRepo.GetCategoryList() };
                return Ok(item);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetReportsList(string reportName, string categoryName)
        {
            try
            {
                ReportsRepository reportsRepo = new ReportsRepository();
                var reportsList = new baseResponse<IEnumerable<Chart>> { responseObject = await reportsRepo.GetChartList(reportName, categoryName) };
                return Ok(reportsList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetFilterQueryLink(string categoryName, string reportName, string chartName, string filterBy)
        {
            try
            {
                ReportsRepository reportRepo = new ReportsRepository();
                var result = await reportRepo.GetFilterQueryLink(categoryName, reportName, chartName, filterBy);
                baseResponse<string> response = new baseResponse<string> { responseObject = result };

                if (string.IsNullOrEmpty(result))
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            ReportsRepository reportRepo = new ReportsRepository();
            var result = await reportRepo.RemoveChart(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IHttpActionResult> CreateTempQuery(string queryName, [FromBody]BaseRequest<string> wiql)
        {
            try
            {
                if (!string.IsNullOrEmpty(queryName) && wiql != null && !string.IsNullOrEmpty(wiql.RequestBody))
                {
                    ReportsRepository reportRepo = new ReportsRepository();
                    var linkString = await reportRepo.GeneratTempQuery(wiql.RequestBody, queryName);
                    baseResponse<string> response = new baseResponse<string> { responseObject = linkString };
                    return Ok(response); 
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class BaseRequest<T>
    {
        public T RequestBody { get; set; }
    }
}
