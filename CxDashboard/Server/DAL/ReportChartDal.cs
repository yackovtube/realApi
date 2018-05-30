using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    public class ReportChartDal
    {
        public IEnumerable<ReportChartModel> GetChartsList(Func<ReportChartModel, bool> func)
        {
            using (DashboardContext dbContext = new DashboardContext())
            {
                return dbContext.ReportCharts.Where(func).ToList();
            }
        }
        public async Task<bool> SaveChart(ReportChartModel chart)
        {
            if (chart != null)
            {
                using (DashboardContext dbContext = new DashboardContext())
                {
                    dbContext.ReportCharts.Add(chart);
                    await dbContext.SaveChangesAsync();
                    return true;
                } 
            }
            return false;
        }
    }
}
