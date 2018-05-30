namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        ReportName = c.String(),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .Index(t => t.Category_CategoryId);
            
            CreateTable(
                "dbo.ReportChartModels",
                c => new
                    {
                        ReportChartId = c.Int(nullable: false, identity: true),
                        ChartName = c.String(nullable: false),
                        QueryId = c.String(nullable: false),
                        ColumnReferenceName = c.String(nullable: false),
                        aggrigationColumnReferenceName = c.String(),
                        AggregationType = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ReportName = c.Int(nullable: false),
                        DataShowType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportChartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.Reports", new[] { "Category_CategoryId" });
            DropTable("dbo.ReportChartModels");
            DropTable("dbo.Reports");
            DropTable("dbo.Categories");
        }
    }
}
