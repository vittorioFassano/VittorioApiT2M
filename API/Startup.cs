using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Data;
using VittorioApiT2M.Application.Services;
using VittorioApiT2M.Domain.Repositories;
using VittorioApiT2M.Infrastructure.Repositories;
using VittorioApiT2M.Infrastructure.Extensions;
using VittorioApiT2M.API.Extensions;

namespace VittorioApiT2M.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Adiciona os controladores MVC e configura os conversores personalizados
            services.AddControllers()
                .AddCustomJsonConverters();

            // Registro de IDbConnection com uma implementação concreta (NpgsqlConnection)
            services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            // Registra os serviços de infraestrutura (repositórios, etc.)
            services.AddInfrastructureServices();
            services.AddScoped<ReservaAppService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Redireciona todas as requisições HTTP para HTTPS
            app.UseStaticFiles(); // Serve arquivos estáticos da pasta wwwroot
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
