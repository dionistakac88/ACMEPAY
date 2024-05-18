using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using FluentMigrator.Runner;
using Persistence;
using Infrastructure.Data;
using Infrastructure;
using Application;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configBuilder)
        {
            services.AddPersistence(configBuilder);
            services.AddApplication();

            return services;
        }
    }
}
