using AutoMetricsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoMetricsService.Infrastructure.Data.Configurations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
            builder.Property(t => t.Price).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasMany(x => x.Taxes)
              .WithOne(x => x.Car)
              .HasForeignKey(x => x.CarId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
