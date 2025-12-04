using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Aplication.Interfaces.Data;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using Core.Framework.Infrastructure.Data;
using Core.Framework.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using RestSharp;

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

        return services;
    }

    public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            DbInitializer.Seed(db);
        }

        return app;
    }
}
