using Microsoft.Extensions.DependencyInjection;
using VittorioApiT2M.Domain.Repositories;
using VittorioApiT2M.Infrastructure.Repositories;
// Outros usings

namespace VittorioApiT2M.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IReservaRepository, ReservaRepository>();

            return services;
        }
    }
}


