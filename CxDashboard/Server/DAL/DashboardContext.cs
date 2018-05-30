using Entities;
using System.Data.Entity;

namespace DAL
{
    class DashboardContext : DbContext
    {
        public DashboardContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportChartModel> ReportCharts { get; set; }
    }
}
