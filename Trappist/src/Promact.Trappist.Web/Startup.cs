using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promact.Trappist.Web.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Repository.TestSettings;
using Promact.Trappist.DomainModel.Seed;
using NLog.Extensions.Logging;
using NLog.Web;
using Promact.Trappist.Repository.Account;
using AutoMapper;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionApplicationClass;


namespace Promact.Trappist.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
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
            services.AddDbContext<TrappistDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Promact.Trappist.Web")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TrappistDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(/*config => { config.Filters.Add(typeof(GlobalExceptionFilter)); }*/);
            services.AddScoped<IQuestionRespository, QuestionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IStringConstants, StringConstants>();
            services.AddScoped<ITestSettingsRepository, TestSettingsRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
          
        
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TrappistDbContext context)
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
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "setup",
                    template: "setup",
                    defaults: new { controller = "Home", action = "setup" });
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
            if (env.IsProduction())
            {
                context.Database.EnsureDeleted();
            }
            context.Database.Migrate();
            context.Seed();
            #region Auto Mapper Configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionApplicationClass>();
                cfg.CreateMap<CodeSnippetQuestion, SingleMultipleAnswerQuestionApplicationClass>();
                cfg.CreateMap<CodeSnippetQuestionDto, CodeSnippetQuestion>();
            });
            #endregion
        }
    }
}