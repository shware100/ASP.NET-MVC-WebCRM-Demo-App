namespace WebCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActivityDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityHistory", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ActivityHistory", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ActivityHistory", "ActivityDate");

            CreateIndex("dbo.ActivityHistory", "StartDate");
            CreateIndex("dbo.ActivityHistory", "EndDate");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityHistory", "ActivityDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ActivityHistory", "EndDate");
            DropColumn("dbo.ActivityHistory", "StartDate");

            DropIndex("dbo.ActivityHistory", "StartDate");
            DropIndex("dbo.ActivityHistory", "EndDate");

        }
    }
}
