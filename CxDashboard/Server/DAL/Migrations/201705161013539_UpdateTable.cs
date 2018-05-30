namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportChartModels", "DataShowType", c => c.Int(nullable: false));
            DropColumn("dbo.ReportChartModels", "DataType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportChartModels", "DataType", c => c.Int(nullable: false));
            DropColumn("dbo.ReportChartModels", "DataShowType");
        }
    }
}
