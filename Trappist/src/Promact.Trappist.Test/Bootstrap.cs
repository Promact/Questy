using System;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.Repository.Profile;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Promact.Trappist.Utility.GlobalUtil;

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
            services.Configure<ConnectionString>(x => x.Value = "connectionstring");
            services.AddScoped(config => config.GetService<IOptionsSnapshot<ConnectionString>>().Value);
            services.Configure<EmailSettings>(x =>
            {
                x.Server = string.Empty;
                x.UserName = string.Empty;
                x.Port = 0;
                x.Password = string.Empty;
                x.ConnectionSecurityOption = string.Empty;
            });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TrappistDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped(config => config.GetService<IOptionsSnapshot<EmailSettings>>().Value);
            services.AddScoped<IQuestionRespository, QuestionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IGlobalUtil, GlobalUtil>();
            return services.BuildServiceProvider();
        }
    }
}
