using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using API.Repository;
using FluentMigrator.Runner;
using Persistence;
using API.Common.Core;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configBuilder)
        {
            services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            return services;
        }
    }
}
