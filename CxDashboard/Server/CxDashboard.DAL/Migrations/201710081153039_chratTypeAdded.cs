namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chratTypeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportChartModels", "ChartType", c => c.Int(nullable: false));
            DropColumn("dbo.ReportChartModels", "AggregationType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportChartModels", "AggregationType", c => c.Int(nullable: false));
            DropColumn("dbo.ReportChartModels", "ChartType");
        }
    }
}
