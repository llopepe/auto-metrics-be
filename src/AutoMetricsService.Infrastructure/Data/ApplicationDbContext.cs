using Core.Framework.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutoMetricsService.Infrastructure.Data
{
    public class ApplicationDbContext : AppDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor accessor)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
    }
}
