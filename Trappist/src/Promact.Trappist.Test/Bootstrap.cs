﻿using AutoMapper;
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
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.EmailServices;
using Promact.Trappist.Utility.FileUtil;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using System.Collections.Generic;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Reports;
using Promact.Trappist.DomainModel.Models.Test;
using Microsoft.Extensions.Configuration;
using Promact.Trappist.Utility.HttpUtil;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EFSecondLevelCache.Core;
using CacheManager.Core;
using Promact.Trappist.Repository.Test;

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
                    options.UseInMemoryDatabase("Questy")
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
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IHttpService, HttpService>();
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

            //Mock IConfiguration
            var configurationMock = new Mock<IConfiguration>();
            var configurationMockObject = configurationMock.Object;
            services.AddScoped(x => configurationMock);
            services.AddScoped(x => configurationMockObject);

            //Mock IHttpService
            var httpServiceMock = new Mock<IHttpService>();
            var httpServiceMockObject = httpServiceMock.Object;
            services.AddScoped(x => httpServiceMock);
            services.AddScoped(x => httpServiceMockObject);
            #endregion

            #region Auto Mapper Configuration

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionDetailAC>();
                cfg.CreateMap<ICollection<Question>, ICollection<QuestionAC>>();
                cfg.CreateMap<DomainModel.Models.Test.Test, TestAC>();
                cfg.CreateMap<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>();
                cfg.CreateMap<DomainModel.Models.Category.Category, CategoryAC>();

                cfg.CreateMap<CodeSnippetQuestionAC, CodeSnippetQuestion>().ForMember(x => x.CodeSnippetQuestionTestCases, opts => opts.Ignore()).ReverseMap();
                cfg.CreateMap<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>().ForMember(x => x.SingleMultipleAnswerQuestionOption, opts => opts.Ignore()).ReverseMap();
                cfg.CreateMap<QuestionDetailAC, Question>().ReverseMap();
                cfg.CreateMap<QuestionAC, Question>().ReverseMap();
                cfg.CreateMap<QuestionDetailAC, Question>();
                cfg.CreateMap<Question, QuestionDetailAC>();
                cfg.CreateMap<ICollection<Question>, ICollection<QuestionAC>>();
                cfg.CreateMap<DomainModel.Models.Test.Test, TestAC>();
                cfg.CreateMap<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>();
                cfg.CreateMap<DomainModel.Models.Category.Category, CategoryAC>();
                cfg.CreateMap<CategoryAC, DomainModel.Models.Category.Category>();
                cfg.CreateMap<TestIpAddress, TestIpAddressAC>();
            });

            services.AddSingleton(mapper.CreateMapper());
            
            #endregion

            services.AddEFSecondLevelCache();

            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                        .WithJsonSerializer()
                        .WithMicrosoftMemoryCacheHandle()
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                        .Build());

            return services.BuildServiceProvider();
        }
    }
}
