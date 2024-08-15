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
using VittorioApiT2M.Application.Data;

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
            services.AddControllers()
                .AddCustomJsonConverters();

            services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            // Registrar a interface IDapperWrapper e sua implementação DapperWrapper
            services.AddScoped<IDapperWrapper, DapperWrapper>();

            // Registrar o repositório IReservaRepository e sua implementação ReservaRepository
            services.AddScoped<IReservaRepository, ReservaRepository>();

            // Registrar outros serviços de infraestrutura
            services.AddInfrastructureServices();

            // Registrar o serviço de aplicação ReservaAppService
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
