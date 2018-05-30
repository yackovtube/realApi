namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainDashboard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationOlderVersionGrades",
                c => new
                    {
                        ApplicationVersionGradeId = c.Int(nullable: false, identity: true),
                        ApplicationVersion = c.Double(nullable: false),
                        RC1QuelityGrade = c.Double(nullable: false),
                        GAQuelityGrade = c.Double(nullable: false),
                        MainDashboard_MainDashboardId = c.Int(),
                    })
                .PrimaryKey(t => t.ApplicationVersionGradeId)
                .ForeignKey("dbo.MainDashboards", t => t.MainDashboard_MainDashboardId)
                .Index(t => t.MainDashboard_MainDashboardId);
            
            CreateTable(
                "dbo.MainDashboards",
                c => new
                    {
                        MainDashboardId = c.Int(nullable: false, identity: true),
                        Timeline = c.Binary(),
                    })
                .PrimaryKey(t => t.MainDashboardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationOlderVersionGrades", "MainDashboard_MainDashboardId", "dbo.MainDashboards");
            DropIndex("dbo.ApplicationOlderVersionGrades", new[] { "MainDashboard_MainDashboardId" });
            DropTable("dbo.MainDashboards");
            DropTable("dbo.ApplicationOlderVersionGrades");
        }
    }
}
