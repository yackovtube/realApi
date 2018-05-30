namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixSpelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportChartModels", "aggregationColumnReferenceName", c => c.String());
            DropColumn("dbo.ReportChartModels", "aggrigationColumnReferenceName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportChartModels", "aggrigationColumnReferenceName", c => c.String());
            DropColumn("dbo.ReportChartModels", "aggregationColumnReferenceName");
        }
    }
}
