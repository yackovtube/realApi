namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationOldGradeDateTimeOfSet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "RC1DateTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ApplicationOlderVersionGrades", "GADateTime", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationOlderVersionGrades", "GADateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApplicationOlderVersionGrades", "RC1DateTime", c => c.DateTime(nullable: false));
        }
    }
}
