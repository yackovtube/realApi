using Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL
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
    }
}
