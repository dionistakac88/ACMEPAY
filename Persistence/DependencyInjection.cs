using Application.Data;
using Domain.Transaction;
using FluentMigrator.Runner;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(config =>
                    config.AddSqlServer()
                    .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(DbConnectionProvider).Assembly).For.All())
                    .AddLogging(config => config.AddFluentMigratorConsole());

            services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
