namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationGradeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "ApplicationVersion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "ApplicationVersion", c => c.String());
        }
    }
}
