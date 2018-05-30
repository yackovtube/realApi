using CxDashboard.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CxDashboard.DAL
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

        public ReportChartModel GetChartByNameAndCartegoryReportIds(int categoryId, int reportId, string ChartName)
        {
            using (var dbContext = new DashboardContext())
            {
                var chart = dbContext.ReportCharts.FirstOrDefault(c => c.ChartName == ChartName && 
                c.CategoryId == categoryId && c.ReportName == reportId);

                if (chart != null)
                {
                    return chart;
                }
                return null;
            }
        }

        public void UpdateChart(int id, ReportChartModel report)
        {
            using (DashboardContext dbContext = new DashboardContext())
            {
                var chart = dbContext.ReportCharts.FirstOrDefault(c => c.ReportChartId == id);
                if (chart != null)
                {

                    dbContext.Entry(chart).CurrentValues.SetValues(report);

                    dbContext.SaveChanges();
                }
            }
        }

        public async Task<bool> DeleteChart(int id)
        {
            using (var dbContext = new DashboardContext())
            {
                var chart = dbContext.ReportCharts.FirstOrDefault(c => c.ReportChartId == id);

                if (chart != null)
                {
                    dbContext.ReportCharts.Remove(chart);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
    }
}
