using System;
using System.IO;
using System.Reflection;
using Extended.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Extended.WebApi.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    // Add Global Exception Filter.
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                // With netcore 3 AddJsonOptions ==> AddNewtonsoftJson
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy =  new SnakeCaseNamingStrategy()
                    };
                    options.SerializerSettings.Converters.Add(
                        new Newtonsoft.Json.Converters.StringEnumConverter(typeof(SnakeCaseNamingStrategy))
                        );
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Location"));
            });

            return services;
        }
        
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details.",
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" },
                    };
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc(configuration["SwaggerDoc:Name"], 
                    new OpenApiInfo
                    {
                        Version = configuration["SwaggerDoc:Version"],
                        Title = configuration["SwaggerDoc:Title"],
                        Description = configuration["SwaggerDoc:Description"],
                        TermsOfService = new Uri(configuration["SwaggerDoc:TermsOfService"]),
                        Contact = new OpenApiContact
                        {
                            Name = configuration["SwaggerDoc:Contact:Name"],
                            Email = string.Empty,
                            Url = new Uri(configuration["SwaggerDoc:Contact:Url"]),
                        },
                        License = new OpenApiLicense
                        {
                            Name = configuration["SwaggerDoc:License:Name"],
                            Url = new Uri(configuration["SwaggerDoc:License:Url"]),
                        }
                    }
                    );
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            return services;
        }
    }
}