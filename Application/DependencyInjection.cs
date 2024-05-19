using Application.Data;
using Application.Mapper;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}
