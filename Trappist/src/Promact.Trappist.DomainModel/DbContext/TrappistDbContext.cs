using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Web.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        }

        /// <summary>
        /// This method used for providing dynamic connection string from user at runtime
        /// </summary>
        /// <param name="optionBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            /*
            While adding migrations, it checks for configured database, but
            at that time, we would be having empty connection string as database is 
            not yet setup. While adding migration, it does not check for any specific connectionstring
            but non empty string. So for that purpose, we are adding fake string. Once db is setup
            it will take connectionstring provided by user
            */
            var fakeConnectionStringToAddMigrations = "fakeConnectionString";
            if (!string.IsNullOrEmpty(_connectionString.Value))
                optionBuilder.UseSqlServer(_connectionString.Value, x => x.MigrationsAssembly("Promact.Trappist.Web"));
            else
                optionBuilder.UseSqlServer(fakeConnectionStringToAddMigrations, x => x.MigrationsAssembly("Promact.Trappist.Web"));
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
        public DbSet<CodeSnippetQuestionTestCases> CodeSnippetQuestionTestCases { get; set; }
        public DbSet<TestAttendees> TestAttendees { get; set; }
        public DbSet<TestCategory> TestCategory { get; set; }
        public DbSet<TestQuestion> TestQuestion { get; set; }
        public DbSet<TestConduct> TestConduct { get; set; }
        public DbSet<TestAnswers> TestAnswers { get; set; }
        public DbSet<AttendeeAnswers> AttendeeAnswers { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<TestLogs> TestLogs { get; set; }
      
        public DbSet<TestIpAddress> TestIpAddresses { get; set; } 
        public DbSet<TestCodeSolution> TestCodeSolution { get; set; }
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
