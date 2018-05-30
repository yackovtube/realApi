namespace CxDashboard.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationOldGradeDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationOlderVersionGrades", "RC1DateTime", c => c.DateTimeOffset(nullable: false));
            AddColumn("dbo.ApplicationOlderVersionGrades", "GADateTime", c => c.DateTimeOffset(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationOlderVersionGrades", "GADateTime");
            DropColumn("dbo.ApplicationOlderVersionGrades", "RC1DateTime");
        }
    }
}
