﻿using AutoMapper;
using Exceptionless;
using Promact.Trappist.Core.TrappistHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Seed;
using Promact.Trappist.Repository.BasicSetup;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Profile;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Reports;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.EmailServices;
using Promact.Trappist.Utility.FileUtil;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Utility.HttpUtil;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Identity;
using CacheManager.Core;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.Test;

namespace Promact.Trappist.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"setup.json", optional: true, reloadOnChange: true);


            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            var threadCountInString = Configuration.GetSection("ThreadCount").Value;

            var threadCount = threadCountInString != null ? Convert.ToInt32(threadCountInString) : 200;

            ThreadPool.SetMinThreads(threadCount, threadCount);

            Env = env;
        }
        public IConfigurationRoot Configuration { get; }

        public IHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.           
            services.AddDbContext<TrappistDbContext>();
            services.AddSignalR();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TrappistDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Trappist.session";
                options.IdleTimeout = TimeSpan.FromDays(1);
            });

            #region Dependencies
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IBasicSetupRepository, BasicSetupRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGlobalUtil, GlobalUtil>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IDbUtility, DbUtility>();
            services.AddScoped<IFileUtility, FileUtility>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<ITestConductRepository, TestConductRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<CodeSnippetQuestionAC, CodeSnippetQuestion>()
                    .ForMember(x => x.CodeSnippetQuestionTestCases, opts => opts.Ignore())
                    .ReverseMap();
                cfg.CreateMap<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>().ForMember(x => x.SingleMultipleAnswerQuestionOption, opts => opts.Ignore()).ReverseMap();
                cfg.CreateMap<QuestionDetailAC, Question>().ReverseMap();
                cfg.CreateMap<QuestionAC, Question>().ReverseMap();
                cfg.CreateMap<Question, QuestionDetailAC>();
                cfg.CreateMap<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>();
                cfg.CreateMap<Category, CategoryAC>();
                cfg.CreateMap<CategoryAC, Category>();
                cfg.CreateMap<CodeSnippetQuestion, CodeSnippetQuestionAC>();
                cfg.CreateMap<Question, QuestionAC>();
                cfg.CreateMap<Test, TestAC>();
                cfg.CreateMap<TestAC, Test>();
                cfg.CreateMap<TestIpAddress, TestIpAddressAC>();
            });

            #endregion

            #region Options configuration
            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionString"));
            services.AddScoped(config => config.GetService<IOptionsSnapshot<ConnectionString>>()?.Value);
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped(config => config.GetService<IOptionsSnapshot<EmailSettings>>()?.Value);
            #endregion

            if (Env.IsDevelopment())
            {
                services.AddMemoryCache();
                services.AddMiniProfiler().AddEntityFramework();
            }

            services.AddLogging();

            

           

            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            // Add Redis cache service provider
            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            services.AddSignalR();
            services.AddDatabaseDeveloperPageExceptionFilter();
            
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                    .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
                    .WithUpdateMode(CacheUpdateMode.Up)
                    .WithRedisConfiguration("redis", Configuration.GetSection("RedisUrl").Value)
                    .WithMaxRetries(100)
                    .WithRetryTimeout(50)
                    .WithRedisCacheHandle("redis")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .Build());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, TrappistDbContext context, ConnectionString connectionString, IMemoryCache cache)
        {
            //app.UseExceptionless(Configuration.GetSection("ExceptionlessKey").Value);
            //using (var loggerBuilder = LoggerFactory.Create(builder=> builder.AddConsole().AddDebug()))
            //{
                
            //}
            //loggerBuilder.AddConsole();
            //loggerBuilder.AddDebug();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();



            if (env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.GetFullPath(Path.Combine(env.ContentRootPath, "node_modules"))),
                    RequestPath = new PathString("/node_modules")
                });
            }

            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();
            }

            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TrappistHub>("/TrappistHub");
                endpoints.MapControllerRoute(
                    name: "pagenotfound",
                    pattern: "pagenotfound",
                    defaults: new { controller = "Home", action = "PageNotFound" });
                endpoints.MapControllerRoute(
                    name: "conduct",
                    pattern: "conduct/{link?}/{route?}",
                    defaults: new { controller = "Home", action = "Conduct" });
                endpoints.MapControllerRoute(
                    name: "setup",
                    pattern: "setup",
                    defaults: new { controller = "Home", action = "Setup" });
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "login",
                    defaults: new { controller = "Account", action = "Login" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToController("Index", "Home");
            });
            

            app.UseSession();


            if (!string.IsNullOrEmpty(connectionString.Value))
            {
                context.Database.Migrate();
                context.Seed();
            }
            #region Auto Mapper Configuration
            
            
            #endregion

            //app.UseEFSecondLevelCache();
        }
    }
}