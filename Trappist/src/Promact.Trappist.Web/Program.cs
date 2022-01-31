using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using System;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

namespace Promact.Trappist.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                Log.Information("Starting web host");
                //you must must have a method/function name "BuildWebHost" otherwise EF Core Migrations will throw an execption that you need to have an implementation of IDesignTimeDbContextFactory
                //BuildWebHost(args).Run();
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    var webHostBuilder = WebHost.CreateDefaultBuilder(args)
        //            .UseStartup<Startup>()
        //            .UseSerilog((hostingContext, loggerConfiguration) =>
        //            {
        //                loggerConfiguration.Enrich.FromLogContext();
        //                loggerConfiguration.WriteTo.Console();

        //                //Just console logs (min level: debug) for development environment
        //                if (hostingContext.HostingEnvironment.IsDevelopment())
        //                {
        //                    loggerConfiguration.MinimumLevel.Debug()
        //                .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
        //                }

        //                //(min level: warning) for production environment
        //                else
        //                {
        //                    loggerConfiguration.MinimumLevel.Warning();
        //                }
        //            });

        //    return webHostBuilder.Build();
        //}


        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("https://*:5001");

            webHostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.Enrich.FromLogContext();

                //Just console logs for development environment
                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.WriteTo.Console();
                    loggerConfiguration.WriteTo.Debug();

                    loggerConfiguration.MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
                }

                //(min level: based on app settings) for production environment
                else
                {
                    loggerConfiguration.WriteTo.Console();
                    loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Error);

                    loggerConfiguration.MinimumLevel.Is(Enum.Parse<LogEventLevel>(hostingContext.Configuration.GetSection("LoggingLevel").Value, true));
                }
            });

            webHostBuilder.ConfigureKestrel((ctx, options) =>
            {
                options.ListenAnyIP(5001, (cfg) =>
                {
                    cfg.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                });
            });

            return webHostBuilder;
        }

    }
}
