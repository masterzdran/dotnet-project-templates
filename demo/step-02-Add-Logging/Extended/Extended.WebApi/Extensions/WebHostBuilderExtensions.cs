using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Extended.WebApi.Extensions
{
    public static class WebHostBuilderExtensions
    {
        private const string  SerilogConfigFile = "Configs/serilog.config.json";
        public static IWebHostBuilder UseSerilogAsLogger(this IWebHostBuilder webBuilder)
        {
            webBuilder
                .CaptureStartupErrors(true)
                .ConfigureAppConfiguration(config =>
                {
                    config
                        .AddJsonFile(SerilogConfigFile, optional: true);
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                loggerConfiguration.Enrich.WithProperty("AppVersion", Assembly.GetEntryAssembly().GetName().Version);
                // Add Log events to System.Diagnostics.Trace
                loggerConfiguration.WriteTo.Trace();
            });
            return webBuilder;
        }
    }
}