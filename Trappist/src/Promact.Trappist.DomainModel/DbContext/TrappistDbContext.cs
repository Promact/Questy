using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.Models.Question;


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


        public DbSet<SingleMultipleAnswerQuestion> SingleMultipleAnswerQuestion { get; set; }
        public DbSet<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }
    }
}
