using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.DomainModel.Models.Category;
using System;
using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.DomainModel.DbContext
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
        public DbSet<Test> Test { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SingleMultipleAnswerQuestion> SingleMultipleAnswerQuestion { get; set; }
        public DbSet<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }

        public DbSet<CodeSnippetQuestion> CodeSnippetQuestion { get; set; }
        public DbSet<CodingLanguage> CodingLanguage { get; set; }
        public DbSet<QuestionLanguageMapping> QuestionLanguageMapping { get; set; }

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
