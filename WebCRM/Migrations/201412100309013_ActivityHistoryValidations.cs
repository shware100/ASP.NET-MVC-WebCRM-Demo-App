namespace WebCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityHistoryValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActivityHistory", "ActivityType", c => c.Int(nullable: false));
            AlterColumn("dbo.ActivityHistory", "ActivityStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.ActivityHistory", "Subject", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActivityHistory", "Subject", c => c.String());
            AlterColumn("dbo.ActivityHistory", "ActivityStatus", c => c.Int());
            AlterColumn("dbo.ActivityHistory", "ActivityType", c => c.Int());
        }
    }
}
