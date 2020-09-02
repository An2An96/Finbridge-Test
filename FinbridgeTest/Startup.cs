using Finbridge.Test.Configuration;
using Finbridge.Test.Middleware;
using Finbridge.Test.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Finbridge.Test
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
            services.Configure<CalculationConfiguration>(Configuration.GetSection("Calculation"));

            services.AddMemoryCache();

            services.AddSingleton<SumOfSquaresService>();
            services.AddSingleton<ISumOfSquares>(p =>
                new SumOfSquaresCacheDecorator(
                    p.GetRequiredService<SumOfSquaresService>(),
                    p.GetRequiredService<IMemoryCache>()));

            services.AddControllers();
            
            services.AddSwaggerGen(o => o.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Finbridge Test Api"
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(c => c
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
