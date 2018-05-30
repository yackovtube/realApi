using CxDashboard.Entities;
using System.Data.Entity;

namespace CxDashboard.DAL
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
        public DbSet<MainDashboard> MainDashboard { get; set; }
        public DbSet<ApplicationOlderVersionGrade> ApplicationOlderVersions { get; set; }
        public DbSet<EngineVersion> EngineVersions { get; set; }
    }
}
