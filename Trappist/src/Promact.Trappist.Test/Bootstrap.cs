using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Moq;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.BasicSetup;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Profile;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.EmailServices;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System;

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
                           .UseInternalServiceProvider(serviceProvider)
                           .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                }, ServiceLifetime.Transient);

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
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IBasicSetupRepository, BasicSetupRepository>();
            services.AddScoped<IGlobalUtil, GlobalUtil>();
            var emailSettingsMock = new Mock<IEmailService>();
            var emailSettingsMockObject = emailSettingsMock.Object;
            services.AddScoped(x => emailSettingsMock);
            services.AddScoped(x => emailSettingsMockObject);
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            var environmentMockObject = hostingEnvironmentMock.Object;
            services.AddScoped(x => hostingEnvironmentMock);
            services.AddScoped(x => environmentMockObject);

            #region Auto Mapper Configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CodeSnippetQuestionAC, CodeSnippetQuestion>();
                cfg.CreateMap<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>();
                cfg.CreateMap<QuestionDetailAC, Question>();
            });
            #endregion

            return services.BuildServiceProvider();
        }
    }
}
