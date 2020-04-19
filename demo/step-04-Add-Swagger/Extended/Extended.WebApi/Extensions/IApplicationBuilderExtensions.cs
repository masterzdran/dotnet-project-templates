using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Extended.WebApi.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseCustomSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(configuration["SwaggerDoc:SwaggerEndpoint"], configuration["SwaggerDoc:ApiName"]);
            });
        }
    }
}