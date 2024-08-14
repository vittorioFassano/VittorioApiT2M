using Microsoft.Extensions.DependencyInjection;
using VittorioApiT2M.Domain.Repositories;
using VittorioApiT2M.Infrastructure.Repositories;

namespace VittorioApiT2M.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //contêiner de injeção de dependência
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            return services;
        }
    }
}

