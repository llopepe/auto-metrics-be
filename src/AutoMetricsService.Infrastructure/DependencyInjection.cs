using AutoMetricsService.Application.Interfaces.Repositories;
using AutoMetricsService.Infrastructure.Data;
using AutoMetricsService.Infrastructure.Repositories;
using Core.Framework.Aplication.Interfaces.Data;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using Core.Framework.Infrastructure.Data;
using Core.Framework.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DB InMemory
        services.AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseInMemoryDatabase("AutoMetricsDb"));

        // Base framework 
        services.AddScoped<AppDbContext, ApplicationDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // Repositories
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICarTaxRepository, CarTaxRepository>();
        services.AddScoped<ICenterRepository, CenterRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();

        return services;
    }

    public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            DbInitializer.Seed(db, logger);
        }

        return app;
    }
}
