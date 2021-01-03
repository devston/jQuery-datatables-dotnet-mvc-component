using Microsoft.AspNet.Identity.EntityFramework;
using MVCDatatables.Data.Models;
using System.Data.Entity;

namespace MVCDatatables.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DummyTable> DummyData { get; set; }
    }
}
