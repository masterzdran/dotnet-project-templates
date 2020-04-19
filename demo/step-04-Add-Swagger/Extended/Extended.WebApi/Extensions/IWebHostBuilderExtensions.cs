using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Extended.WebApi.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        private const string SerilogConfigFile = "Configs/serilog.config.json";
        private const string SwaggerConfigFile = "Configs/swagger.config.json";
        public static IWebHostBuilder UseSerilogAsLogger(this IWebHostBuilder webBuilder)
        {
            webBuilder
                .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                loggerConfiguration.Enrich.WithProperty(
                    "AppVersion", 
                    Assembly.GetEntryAssembly().GetName().Version);
                // Add Log events to System.Diagnostics.Trace
                loggerConfiguration.WriteTo.Trace();
            });
            return webBuilder;
        }

        public static IWebHostBuilder UseCustomConfiguration(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder
                .ConfigureAppConfiguration(config =>
                {
                    config
                        .AddJsonFile(SerilogConfigFile, optional: true)
                        .AddJsonFile(SwaggerConfigFile,true);
                });
            return webHostBuilder;
        }
    }
}