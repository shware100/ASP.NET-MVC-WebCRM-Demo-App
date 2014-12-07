using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebCRM.Models;

namespace WebCRM.DAL
{
    public class WebCRMDBContext : DbContext
    {
        public WebCRMDBContext()
            : base("WebCRMDBContext")
        {

        }
        public DbSet<Company> Companies { get; set; }
        // public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ActivityHistory> ActivityHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Contacts)
                .WithMany(i => i.Companies)
                .Map(t => t.MapLeftKey("CompanyID")
                .MapRightKey("ContactID")
                .ToTable("CompanyContact"));
        }
    }
}
 