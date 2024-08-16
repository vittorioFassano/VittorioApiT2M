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

            services.AddScoped<IDapperWrapper, DapperWrapper>();
            services.AddScoped<IReservaRepository, ReservaRepository>();

            // Registrar a interface IReservaAppService e sua implementação ReservaAppService
            services.AddScoped<IReservaAppService, ReservaAppService>();

            // Adicionar o serviço Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Vittorio API",
                    Version = "v1",
                    Description = "API para gerenciar reservas"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vittorio API v1");
                    c.RoutePrefix = string.Empty;
                });
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
