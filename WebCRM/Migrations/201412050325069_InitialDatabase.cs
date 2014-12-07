namespace WebCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityHistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActivityType = c.Int(),
                        ActivityStatus = c.Int(),
                        CompanyID = c.Int(nullable: false),
                        ContactID = c.Int(nullable: false),
                        ActivityDate = c.DateTime(nullable: true),
                        Subject = c.String(),
                        Comments = c.String(),
                        CreatedAt = c.DateTime(nullable: true),
                        CreatedByID = c.Int(nullable: true),
                        Updatedat = c.DateTime(nullable: true),
                        UpdatedByID = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.ContactID);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        CityRegion = c.String(),
                        StateProvince = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        CreatedAt = c.DateTime(nullable: true),
                        CreatedByID = c.Int(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        UpdatedByID = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Honorific = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Suffix = c.String(),
                        Title = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        AlternateEmail = c.String(),
                        CreatedAt = c.DateTime(nullable: true),
                        CreatedByID = c.Int(nullable: true),
                        UpdatedAt = c.DateTime(nullable: true),
                        UpdatedByID = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CompanyContact",
                c => new
                    {
                        CompanyID = c.Int(nullable: false),
                        ContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyID, t.ContactID })
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.ContactID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyContact", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.CompanyContact", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.ActivityHistory", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.ActivityHistory", "CompanyID", "dbo.Company");
            DropIndex("dbo.CompanyContact", new[] { "ContactID" });
            DropIndex("dbo.CompanyContact", new[] { "CompanyID" });
            DropIndex("dbo.ActivityHistory", new[] { "ContactID" });
            DropIndex("dbo.ActivityHistory", new[] { "CompanyID" });
            DropTable("dbo.CompanyContact");
            DropTable("dbo.Contact");
            DropTable("dbo.Company");
            DropTable("dbo.ActivityHistory");
        }
    }
}
