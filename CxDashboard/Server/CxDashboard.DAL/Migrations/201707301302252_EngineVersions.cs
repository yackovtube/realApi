namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EngineVersions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EngineVersions",
                c => new
                    {
                        VersionId = c.Int(nullable: false, identity: true),
                        EngineVersionNumber = c.String(),
                        IsCurrentVersion = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VersionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EngineVersions");
        }
    }
}
