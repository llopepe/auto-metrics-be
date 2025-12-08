using AutoMetricsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoMetricsService.Infrastructure.Data.Configurations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Units).IsRequired();
            builder.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.TotalTax).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.Total).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(s => s.Car)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CarId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Center)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CenterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
