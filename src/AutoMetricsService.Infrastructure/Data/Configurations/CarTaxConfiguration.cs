using AutoMetricsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Infrastructure.Data.Configurations
{
    public class CarTaxConfiguration : IEntityTypeConfiguration<CarTax>
    {
        public void Configure(EntityTypeBuilder<CarTax> builder)
        {
            builder.ToTable("CarTaxs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Percentage).HasPrecision(5, 2).IsRequired();
        }
    }
}