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
using Promact.Trappist.Utility.FileUtil;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;

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

            #region Context and Identity setup
            services.AddEntityFrameworkInMemoryDatabase().
                AddDbContext<TrappistDbContext>((serviceProvider, options) =>
                {
                    options.UseInMemoryDatabase()
                           .UseInternalServiceProvider(serviceProvider)
                           .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<TrappistDbContext>()
               .AddDefaultTokenProviders();
            #endregion

            #region Setup parameters
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
            services.AddScoped(config => config.GetService<IOptionsSnapshot<EmailSettings>>().Value);
            #endregion

            #region Dependencies       
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<IBasicSetupRepository, BasicSetupRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IBasicSetupRepository, BasicSetupRepository>();
            services.AddScoped<IGlobalUtil, GlobalUtil>();
            services.AddScoped<ITestConductRepository, TestConductRepository>();
            #endregion

            #region Mocking
            //Mock IHostingEnvironment
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            var environmentMockObject = hostingEnvironmentMock.Object;
            services.AddScoped(x => hostingEnvironmentMock);
            services.AddScoped(x => environmentMockObject);

            //Mock SqlConnection
            var sqlConnectionMock = new Mock<IDbUtility>();
            var sqlConnectionObject = sqlConnectionMock.Object;
            services.AddScoped(x => sqlConnectionMock);
            services.AddScoped(x => sqlConnectionObject);

            //Mock write json file
            var wrtieJsonFileMock = new Mock<IFileUtility>();
            var wrtieJsonFileMockObject = wrtieJsonFileMock.Object;
            services.AddScoped(x => wrtieJsonFileMock);
            services.AddScoped(x => wrtieJsonFileMockObject);

            //Mock email settings
            var emailSettingsMock = new Mock<IEmailService>();
            var emailSettingsMockObject = emailSettingsMock.Object;
            services.AddScoped(x => emailSettingsMock);
            services.AddScoped(x => emailSettingsMockObject);

            //Mock global utility
            var globalUtilMock = new Mock<IGlobalUtil>();
            var globalUtilObject = globalUtilMock.Object;
            services.AddScoped(x => globalUtilMock);
            services.AddScoped(x => globalUtilObject);
            #endregion

            #region Auto Mapper Configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CodeSnippetQuestionAC, CodeSnippetQuestion>().ForMember(x => x.CodeSnippetQuestionTestCases, opts => opts.Ignore()).ReverseMap();
                cfg.CreateMap<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>().ReverseMap();
                cfg.CreateMap<QuestionDetailAC, Question>().ReverseMap();
                cfg.CreateMap<QuestionAC, Question>().ReverseMap();
                cfg.CreateMap<DomainModel.Models.Category.Category, CategoryAC>();
                cfg.CreateMap<DomainModel.Models.Test.Test, TestAC>();
            });
            #endregion

            return services.BuildServiceProvider();
        }
    }
}
