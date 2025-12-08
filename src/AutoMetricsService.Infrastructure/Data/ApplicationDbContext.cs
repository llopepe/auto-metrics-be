using AutoMetricsService.Domain.Entities;
using AutoMetricsService.Infrastructure.Data.Configurations;
using Core.Framework.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutoMetricsService.Infrastructure.Data
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApplicationDbContext : AppDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor accessor)
            : base(options)
        {

        }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Center> Centers => Set<Center>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<CarTax> CarTaxes => Set<CarTax>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration ());
            modelBuilder.ApplyConfiguration(new CenterConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new CarTaxConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
