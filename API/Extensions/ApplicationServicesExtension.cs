using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using Domain.Interfaces;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services) // 2611
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options => // 2611
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin() // WithOrigins("https://domain.com")
                .AllowAnyMethod() // WithMethods("GET", "POST")
                .AllowAnyHeader(); // WithHeaders("accept", "content-type")
            });
        });

        public static void ConfigureRateLimiting(this IServiceCollection services) // 2611
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",  // Si quiere usar todos ponga *
                        Period = "10s", // Periodo de tiempo para hacer peticiones
                        Limit = 10         // Numero de peticiones durante el periodo de tiempo
                    }
                };
            });
        }
    }
}