using CxDashboard.BL;
using CxDashboard.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Http;

namespace CxDashboard.server.Controllers
{
    [Authorize]
    public class SettingsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetCategoryList()
        {
            try
            {
                var settingsRepo = new SettingsRepository();
                return Ok(settingsRepo.GetCategoryList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetReportsAndQueriesName(string categoryId)
        {
            try
            {
                var settingsRepo = new SettingsRepository();
                var result = await settingsRepo.GetQueriesNReportsNameList(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetQueryData(string queryId)
        {
            try
            {
                var settingsRepo = new SettingsRepository();
                var result = await settingsRepo.GetQueryColumn(queryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //[HttpGet]
        //public IHttpActionResult GetAggregationType(string aggregationField)
        //{
        //    try
        //    {
        //        var settingsRepo = new SettingsRepository();
        //        var result = new baseResponse<dynamic> { responseObject = settingsRepo.GetColumnType(aggregationField).Result };
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
        [HttpPost]
        public async Task<IHttpActionResult> SaveNewChart([FromBody]ReportChartModel report)
        {
            if (report == null)
            {
                return BadRequest("report cannot be null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var settingsRepo = new SettingsRepository();
            var result = await settingsRepo.AddReportChart(report);
            return Ok();
        }
        [HttpPut]
        public IHttpActionResult UpdateChart(int id, [FromBody]ReportChartModel report)
        {
            try
            {
                if (report == null)
                {
                    return BadRequest("Report cannot be null!");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ReportsRepository reportRepo = new ReportsRepository();
                reportRepo.UpdateChart(id, report);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public IHttpActionResult SaveTimeline([FromBody]fileObj imageBase64)
        {
            if (imageBase64 == null)
            {
                return BadRequest("image cannot be null");
            }
            SettingsRepository settingsRepo = new SettingsRepository();
            settingsRepo.UpdateTimeline(imageBase64.fileBase64);
            return Ok();
        }
        [HttpPut]
        public async Task<IHttpActionResult> SetRC1Grade(string version)
        {
            try
            {
                SettingsRepository settingsRepo = new SettingsRepository();
                var result = await settingsRepo.UpdateRC1GradeAndVersion(version);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPut]
        public async Task<IHttpActionResult> SetGAGrade()
        {
            SettingsRepository settingRepo = new SettingsRepository();
            var result = await settingRepo.UpdateGAGrade();
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }
        [HttpPut]
        public IHttpActionResult SetEngineNewVersion(string engineVersion)
        {
            if (string.IsNullOrEmpty(engineVersion))
            {
                return BadRequest();
            }
            try
            {
                SettingsRepository settingsRepo = new SettingsRepository();
                var result = settingsRepo.UpdateEngineVersion(engineVersion);

                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class fileObj 
    {
        public string fileBase64 { get; set; }

    }

    [Serializable]
    public class baseResponse<T>
    {
        public T responseObject { get; set; }
    }
}
