using AutoMetricsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoMetricsService.Infrastructure.Data.Configurations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(150);
            builder.Property(u => u.IsActive).HasDefaultValue(true);
            builder.Property(u => u.Roles).IsRequired().HasMaxLength(300);

            // Índice único por Email
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
