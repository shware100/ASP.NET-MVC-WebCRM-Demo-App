namespace WebCRM.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebCRM.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WebCRM.DAL.WebCRMDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebCRM.DAL.WebCRMDBContext context)
        {
            context.Companies.AddOrUpdate(
                c => c.ID,
                new Company { ID = 1, Name = "Company 001", Address1 = "100 Main St.", Address2 = "PO Box 100", CityRegion = "Morrisville", StateProvince = "PA", PostalCode = "19067", Phone = "(215) 333-1111", Fax = "(215) 333-1112", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Company { ID = 2, Name = "Company 002", Address1 = "200 Main St.", Address2 = "PO Box 200", CityRegion = "Morrisville", StateProvince = "PA", PostalCode = "19067", Phone = "(215) 333-2222", Fax = "(215) 333-2221", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Company { ID = 3, Name = "Company 003", Address1 = "300 Main St.", Address2 = "PO Box 300", CityRegion = "Morrisville", StateProvince = "PA", PostalCode = "19067", Phone = "(215) 333-3333", Fax = "(215) 333-3332", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Company { ID = 4, Name = "Company 004", Address1 = "400 Main St.", Address2 = "PO Box 400", CityRegion = "Morrisville", StateProvince = "PA", PostalCode = "19067", Phone = "(215) 333-4444", Fax = "(215) 333-4442", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Company { ID = 5, Name = "Company 005", Address1 = "500 Main St.", Address2 = "PO Box 500", CityRegion = "Morrisville", StateProvince = "PA", PostalCode = "19067", Phone = "(215) 333-5555", Fax = "(215) 333-5552", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );

            context.Contacts.AddOrUpdate(
                c => c.ID,
                // company 1 contacts
                new Contact { ID = 1, FirstName = "John", LastName = "Doe", Email = "jdoe@somesite.com", Title = "Doe Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 2, FirstName = "Jack", LastName = "Reacher", Email = "jreacher@somesite.com", Title = "Reacher Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 3, FirstName = "Ron", LastName = "Reagan", Email = "rreagan@somesite.com", Title = "Reagan Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },

                // company 2 contacts
                new Contact { ID = 4, FirstName = "Jane", LastName = "Doe", Email = "jdoe2@somesite.com", Title = "Doe2 Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 5, FirstName = "Jim", LastName = "Kirk", Email = "jkirk@somesite.com", Title = "Kirk Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 6, FirstName = "Hikaru", LastName = "Sulu", Email = "hsulu@somesite.com", Title = "Sulu Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },

                // company 3 contacts
                new Contact { ID = 7, FirstName = "Bugs", LastName = "Bunny", Email = "bbunny@somesite.com", Title = "Bunny Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 8, FirstName = "Road", LastName = "Runner", Email = "rrunner@somesite.com", Title = "Runner Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 9, FirstName = "Porky", LastName = "Pig", Email = "ppig@somesite.com", Title = "Pig Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },

                // company 4 contacts
                new Contact { ID = 10, FirstName = "Bruce", LastName = "Wayne", Email = "bwayne@somesite.com", Title = "Wayne Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 11, FirstName = "Dick", LastName = "Grayson", Email = "jgrayson@somesite.com", Title = "Grayson Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 12, FirstName = "The", LastName = "Riddler", Email = "triddler@somesite.com", Title = "Riddler Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },

                // company 5 contacts
                new Contact { ID = 13, FirstName = "Peggy", LastName = "Smith", Email = "psmith@somesite.com", Title = "Smith Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 14, FirstName = "George", LastName = "Bush", Email = "gbush@somesite.com", Title = "Bush Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Contact { ID = 15, FirstName = "Bill", LastName = "Clinton", Email = "bclinton@somesite.com", Title = "Clinton Title", Phone = "(333) 444-5555", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );

            var today = DateTime.Now;

            context.ActivityHistory.AddOrUpdate(
                a => a.ID,
                new ActivityHistory { ID = 1, CompanyID = 1, ContactID = 1, ActivityType = ActivityType.Call, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-7), EndDate = today.AddDays(-7).AddMinutes(15), Subject = "Company 001 Event 1", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 2, CompanyID = 1, ContactID = 2, ActivityType = ActivityType.Meeting, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-5).AddMinutes(30), EndDate = today.AddDays(-5).AddMinutes(60), Subject = "Company 001 Event 2", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 3, CompanyID = 1, ContactID = 3, ActivityType = ActivityType.Task, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-3).AddHours(5), EndDate = today.AddDays(-3).AddHours(5).AddMinutes(45), Subject = "Company 001 Event 3", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },

                new ActivityHistory { ID = 4, CompanyID = 2, ContactID = 4, ActivityType = ActivityType.Call, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+7).AddHours(-4), EndDate = today.AddDays(+7).AddHours(-4).AddMinutes(15), Subject = "Company 002 Event 1", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 5, CompanyID = 2, ContactID = 5, ActivityType = ActivityType.Meeting, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+5).AddHours(-3), EndDate = today.AddDays(+5).AddHours(-3).AddMinutes(30), Subject = "Company 002 Event 2", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 6, CompanyID = 2, ContactID = 6, ActivityType = ActivityType.Task, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+3).AddHours(-3), EndDate = today.AddDays(+3).AddHours(-3).AddMinutes(45), Subject = "Company 002 Event 3", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },

                new ActivityHistory { ID = 7, CompanyID = 3, ContactID = 7, ActivityType = ActivityType.Call, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-4).AddHours(12), EndDate = today.AddDays(-4).AddHours(12).AddMinutes(15), Subject = "Company 003 Event 1", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 8, CompanyID = 3, ContactID = 8, ActivityType = ActivityType.Meeting, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-2).AddHours(-8), EndDate = today.AddDays(-2).AddHours(-8).AddMinutes(30), Subject = "Company 003 Event 2", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 9, CompanyID = 3, ContactID = 9, ActivityType = ActivityType.Task, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(-1).AddHours(-2), EndDate = today.AddDays(-1).AddHours(-3).AddMinutes(45), Subject = "Company 003 Event 3", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },

                new ActivityHistory { ID = 10, CompanyID = 4, ContactID = 10, ActivityType = ActivityType.Call, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+8), EndDate = today.AddDays(+8).AddMinutes(30), Subject = "Company 004 Event 1", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 11, CompanyID = 4, ContactID = 11, ActivityType = ActivityType.Meeting, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+9), EndDate = today.AddDays(+9).AddMinutes(45), Subject = "Company 004 Event 2", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 12, CompanyID = 4, ContactID = 12, ActivityType = ActivityType.Task, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+10), EndDate = today.AddDays(+10).AddMinutes(60), Subject = "Company 004 Event 3", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },

                new ActivityHistory { ID = 13, CompanyID = 5, ContactID = 13, ActivityType = ActivityType.Call, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+2), EndDate = today.AddDays(+2).AddMinutes(10), Subject = "Company 005 Event 1", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 14, CompanyID = 5, ContactID = 14, ActivityType = ActivityType.Meeting, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+4), EndDate = today.AddDays(+4).AddMinutes(30), Subject = "Company 005 Event 2", CreatedAt = DateTime.Now, Updatedat = DateTime.Now },
                new ActivityHistory { ID = 15, CompanyID = 5, ContactID = 15, ActivityType = ActivityType.Task, ActivityStatus = ActivityStatus.Active, StartDate = today.AddDays(+6), EndDate = today.AddDays(+6).AddMinutes(120), Subject = "Company 005 Event 3", CreatedAt = DateTime.Now, Updatedat = DateTime.Now }

                );

            context.SaveChanges();

            WebCRMMigrationUtils.AddOrUpdateCompanyContacts(context, 1, new int[] { 1, 2, 3 });
            WebCRMMigrationUtils.AddOrUpdateCompanyContacts(context, 2, new int[] { 4, 5, 6 });
            WebCRMMigrationUtils.AddOrUpdateCompanyContacts(context, 3, new int[] { 7, 8, 9 });
            WebCRMMigrationUtils.AddOrUpdateCompanyContacts(context, 4, new int[] { 10, 11, 12 });
            WebCRMMigrationUtils.AddOrUpdateCompanyContacts(context, 5, new int[] { 13, 14, 15 });


        }

        /*
        protected void AddOrUpdateCompanyContacts(WebCRM.DAL.WebCRMDBContext context, int companyID, int[] contactIDs) {
            var company = context.Companies.Find(companyID);
            if (company.Contacts.Count == 0)
            {
                var q =
                    from c in context.Contacts
                    where contactIDs.Contains(c.ID)
                    select c;

                foreach(Contact cn in q.ToList<Contact>()) {
                    company.Contacts.Add(cn);
                }
                context.SaveChanges();
            }
        }
        */
    }

    public static class WebCRMMigrationUtils {

        public static void AddOrUpdateCompanyContacts(WebCRM.DAL.WebCRMDBContext context, int companyID, int[] contactIDs) {
            var company = context.Companies.Find(companyID);
            if (company.Contacts.Count == 0)
            {
                var q =
                    from c in context.Contacts
                    where contactIDs.Contains(c.ID)
                    select c;

                foreach(Contact cn in q.ToList<Contact>()) {
                    company.Contacts.Add(cn);
                }
                context.SaveChanges();
            }
        }
    }
}
