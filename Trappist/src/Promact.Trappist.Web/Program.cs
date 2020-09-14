using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using System;
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
                BuildWebHost(args).Run();
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

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration.Enrich.FromLogContext();
                        loggerConfiguration.WriteTo.Console();

                        //Just console logs (min level: debug) for development environment
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            loggerConfiguration.MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
                        }

                        //(min level: warning) for production environment
                        else
                        {
                            loggerConfiguration.MinimumLevel.Warning();
                        }
                    });

            var appInsightsKey = Environment.GetEnvironmentVariable("ApplicationInsights__InstrumentationKey");

            if (appInsightsKey != null)
            {
                webHostBuilder.UseApplicationInsights(appInsightsKey);
            }

            return webHostBuilder.Build();
        }

    }
}
