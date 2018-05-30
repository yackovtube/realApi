namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class versionAsString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "ApplicationVersion", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "ApplicationVersion", c => c.Double(nullable: false));
        }
    }
}
