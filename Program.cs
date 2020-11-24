using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApiTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Standart way without logging
            //CreateHostBuilder(args).Build().Run();

            //A new way to log a start of the application
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("{Time}:| WebApiTest has Started", DateTime.UtcNow);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logBuilder) =>
                {
                    logBuilder.ClearProviders(); // removes all providers from LoggerFactory
                    logBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));//This Logging comes from appsettings.json
                    logBuilder.AddDebug();
                    logBuilder.AddConsole(); 
                    
                    //logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
                    //There is more options of logging as:
                    //EventSource, EventLog, TraceSourcem, AzureAppServiceFile, AzureAppServiceBlob, ApplicatonInsights
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
