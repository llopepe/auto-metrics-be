using AutoMetricsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoMetricsService.Infrastructure.Data.Configurations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CenterConfiguration : IEntityTypeConfiguration<Center>
    {
        public void Configure(EntityTypeBuilder<Center> builder)
        {
            builder.ToTable("Centers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
        }
    }
}
