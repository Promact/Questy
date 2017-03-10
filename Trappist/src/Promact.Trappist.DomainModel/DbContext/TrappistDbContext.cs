using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models;

namespace Promact.Trappist.Web.Data
{
    public class TrappistDbContext : IdentityDbContext<ApplicationUser>
    {
        public TrappistDbContext(DbContextOptions<TrappistDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Question> Question { get; set; }

        #region Overridden Methods

        public override int SaveChanges()
        {
            ChangeTracker.Entries().Where(x => x.Entity is BaseModel && x.State == EntityState.Added).ToList().ForEach(x =>
            {
                ((BaseModel)x.Entity).CreatedDateTime = DateTime.UtcNow;
            });
            ChangeTracker.Entries().Where(x => x.Entity is BaseModel && x.State == EntityState.Modified).ToList().ForEach(x =>
            {
                ((BaseModel)x.Entity).UpdateDateTime = DateTime.UtcNow;
            });

            return base.SaveChanges();
        }

        #endregion
    }
}
