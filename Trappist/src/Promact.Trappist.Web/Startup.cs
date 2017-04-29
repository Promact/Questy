using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using Promact.Trappist.DomainModel.ApplicationClasses;
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
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.EmailServices;
using Promact.Trappist.Utility.FileUtil;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System;
using System.IO;

namespace Promact.Trappist.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {   
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"setup.json", optional: true, reloadOnChange: true);


            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.           
            services.AddDbContext<TrappistDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TrappistDbContext>()
                .AddDefaultTokenProviders();

            services.AddDirectoryBrowser();
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDirectoryBrowser();

            services.AddSession(options =>
            {
                options.CookieName = ".Trappist.session";
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
            #endregion

            #region Options configuration
            services.Configure<ConnectionString>(Configuration.GetSection("ConnectionString"));
            services.AddScoped(config => config.GetService<IOptionsSnapshot<ConnectionString>>().Value);
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped(config => config.GetService<IOptionsSnapshot<EmailSettings>>().Value);
            #endregion

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TrappistDbContext context, ConnectionString connectionString)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
            app.UseIdentity();
            
            app.UseSession();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "pagenotfound",
                    template: "pagenotfound",
                    defaults: new { controller = "Home", action = "PageNotFound" });
                routes.MapRoute(
                    name: "conduct",
                    template: "conduct/{link?}/{route?}",
                    defaults: new { controller = "Home", action = "Conduct", route = "register" });
                routes.MapRoute(
                    name: "setup",
                    template: "setup",
                    defaults: new { controller = "Home", action = "Setup" });
                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "Account", action = "Login" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapSpaFallbackRoute(
                     name: "spa-fallback",
                     defaults: new { controller = "Home", action = "Index" });
            });

            //Delete production db upon every deployment
            //Temporary fix as we are not including migrations in scm
            //Will remove after we include migrations in code base
            if (!string.IsNullOrEmpty(connectionString.Value))
            {
                context.Database.Migrate();
                context.Seed();
            }
            #region Auto Mapper Configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CodeSnippetQuestionAC, CodeSnippetQuestion>()
                .ForMember(x => x.CodeSnippetQuestionTestCases, opts => opts.Ignore())
                .ReverseMap();
                cfg.CreateMap<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>().ForMember(x => x.SingleMultipleAnswerQuestionOption, opts => opts.Ignore()).ReverseMap();
                cfg.CreateMap<QuestionDetailAC, Question>().ReverseMap();
                cfg.CreateMap<QuestionAC, Question>().ReverseMap();
                cfg.CreateMap<Question, QuestionDetailAC> ();
                cfg.CreateMap<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>();
                cfg.CreateMap<Category, CategoryAC>();
                cfg.CreateMap<CategoryAC, Category>();
                cfg.CreateMap<CodeSnippetQuestion, CodeSnippetQuestionAC>();
                cfg.CreateMap<Question,QuestionAC>();
                cfg.CreateMap<Test, TestAC>();
                cfg.CreateMap<TestAC, Test>();
            });
            #endregion
        }
    }
}
