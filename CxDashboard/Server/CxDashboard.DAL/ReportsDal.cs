using CxDashboard.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CxDashboard.DAL
{
    public class ReportsDal
    {
        public IEnumerable<Report> GetReportsByCategoryId(int categoryId)
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.Reports.Where(r => r.Category.CategoryId == categoryId).ToList();
            }
        }

        public int GetReportIdByNameAndCategoryId(int categoryId, string reportName)
        {
            using (var dbContext = new DashboardContext())
            {
                var report = dbContext.Reports.FirstOrDefault(r => r.ReportName == reportName && r.Category.CategoryId == categoryId);
                if (report != null)
                {
                    return report.ReportId;
                }

                return 0;
            }
        }
    }
}
