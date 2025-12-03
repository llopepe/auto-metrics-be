using Core.Framework.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Core.Framework.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                    var compare = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                    var lambda = Expression.Lambda(compare, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }


            base.OnModelCreating(modelBuilder);
        }

    }
}
