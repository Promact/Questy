using System;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Repository.ProfileDetails;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Promact.Trappist.Test
{
    public class Bootstrap
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public Bootstrap()
        {
            ServiceProvider = BuildServiceProvider();
        }

        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase().
                AddDbContext<TrappistDbContext>((serviceProvider, options) =>
                {
                    options.UseInMemoryDatabase()
                           .UseInternalServiceProvider(serviceProvider);
                },ServiceLifetime.Transient);

            //Register all dependencies here
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<TrappistDbContext>()
               .AddDefaultTokenProviders();
            services.AddScoped<IQuestionRespository, QuestionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            return services.BuildServiceProvider();
        }
    }
}
