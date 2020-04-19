using Extended.WebApi.Extensions;
using Extended.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Extended.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .CaptureStartupErrors(true)  
                        // Add Custom Configuration Files
                        .UseCustomConfiguration()
                        // Add Serilog
                        .UseSerilogAsLogger();
                })
               
        ;
    }
}
