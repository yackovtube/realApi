using CxDashboard.BL;
using CxDashboard.Entities.Responses;
using System.Threading.Tasks;
using System.Web.Http;

namespace CxDashboard.server.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetApplicationGrade()
        {
            ReportsRepository reportsRepo = new ReportsRepository();
            var result = await reportsRepo.GetMainDashboardData();

            return Ok(result);
        }
    }
}
