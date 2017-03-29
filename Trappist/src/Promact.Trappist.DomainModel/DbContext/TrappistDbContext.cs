using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.DomainModel.Models.Category;
using System;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using System.Threading.Tasks;
using System.Threading;

namespace Promact.Trappist.DomainModel.DbContext
{
    public class TrappistDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Private variables
        #region Dependencies
        private readonly ConnectionString _connectionString;
        #endregion
        #endregion

        #region Constructors
        public TrappistDbContext(DbContextOptions<TrappistDbContext> options, ConnectionString connectionString)
             : base(options)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Protected Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
        }

        /// <summary>
        /// This method used for providing dynamic connection string from user at runtime
        /// </summary>
        /// <param name="optionBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString.Value))
                optionBuilder.UseSqlServer(_connectionString.Value,x=>x.MigrationsAssembly("Promact.Trappist.Web"));
            base.OnConfiguring(optionBuilder);
        }
        #endregion

        public DbSet<Test> Test { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SingleMultipleAnswerQuestion> SingleMultipleAnswerQuestion { get; set; }
        public DbSet<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }
        public DbSet<Question> Question { get; set; }
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
        /// <summary>
        /// override savechangesasync method
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.Entries().Where(x => x.Entity is BaseModel && x.State == EntityState.Added).ToList().ForEach(x =>
            {
                ((BaseModel)x.Entity).CreatedDateTime = DateTime.UtcNow;
            });
            ChangeTracker.Entries().Where(x => x.Entity is BaseModel && x.State == EntityState.Modified).ToList().ForEach(x =>
            {
                ((BaseModel)x.Entity).UpdateDateTime = DateTime.UtcNow;
            });
            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
