using AutoMetricsService.Infrastructure.Data;
using Core.Framework.Aplication.Interfaces.Data;
using Core.Framework.Aplication.Interfaces.Repositories.Base;
using Core.Framework.Infrastructure.Data;
using Core.Framework.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PrimaryDbConnection");
        var commandTimeout = configuration.GetValue<int>("CommandTimeout");

        Guard.Against.Null(connectionString, message: "Connection string 'PrimaryDbConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString,
                 sqlServerOptions => sqlServerOptions.CommandTimeout(commandTimeout))
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true);
        });

        // Base framework 
        services.AddScoped<AppDbContext, ApplicationDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // Repositories


        return services;
    }
}
