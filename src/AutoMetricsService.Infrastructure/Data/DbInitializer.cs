using AutoMetricsService.Domain.Entities;
using Core.Framework.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMetricsService.Infrastructure.Data
{
    internal class DbInitializer
    {
        public static void Seed(ApplicationDbContext db, ILogger logger)
        {
            logger.LogInformation("Checking database status...");
            var created = db.Database.EnsureCreated();

            if (created)
                logger.LogInformation("Database created (EnsureCreated).");
            else
                logger.LogInformation("Database already exists.");

            logger.LogInformation("Seeding database...");
 

            if (!db.Centers.Any())
            {
                SeedCenters(db);
                logger.LogInformation("Centers seeded.");
            }

            if (!db.Cars.Any())
            {
                SeedCars(db);
                logger.LogInformation("Cars seeded.");
            }

            if (!db.CarTaxes.Any())
            {
                SeedCarTaxes(db);
                logger.LogInformation("CarTaxes seeded.");
            }

            if (!db.Sales.Any())
            {
                SeedSales(db);
                logger.LogInformation("Sales seeded.");
            }


            logger.LogInformation("Database ready.");
        }

        private static void SeedCars(ApplicationDbContext db)
        {
            if (db.Cars.Any()) return;

            db.Cars.AddRange(
                new Car { Id = 1, Name = "Sedan", Price = 8000 },
                new Car { Id = 2, Name = "SUV", Price = 9500 },
                new Car { Id = 3, Name = "Offroad", Price = 12500 },
                new Car { Id = 4, Name = "Sport", Price = 18200 }
            );

            db.SaveChanges();
        }

        private static void SeedCenters(ApplicationDbContext db)
        {
            if (db.Centers.Any()) return;

            db.Centers.AddRange(
                new Center { Id = 1, Name = "Centro Norte" },
                new Center { Id = 2, Name = "Centro Sur"},
                new Center { Id = 3, Name = "Distribuidor Andes" },
                new Center { Id = 4, Name = "Costa Premium" }
            );

            db.SaveChanges();
        }

        private static void SeedSales(ApplicationDbContext db)
        {
            if (db.Sales.Any()) return;

            var sales = new List<Sale>
            {
                new Sale
                {
                    Id = 1,
                    CenterId = 1,
                    CarId = 1, // Sedan
                    Units = 3,
                    UnitPrice = 8000m,
                    TotalTax = 0,
                    Total = 3 * 8000m,
                    Date = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"

                },
                new Sale
                {
                    Id = 2,
                    CenterId = 2,
                    CarId = 2, // SUV
                    Units = 4,
                    UnitPrice = 9500m,
                    TotalTax = 0,
                    Total = 4 * 9500m,
                    Date = new DateTimeOffset(2025, 1, 25, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 3,
                    CenterId = 3,
                    CarId = 3, // Offroad
                    Units = 2,
                    UnitPrice = 12500m,
                    TotalTax = 0,
                    Total = 2 * 12500m,
                    Date = new DateTimeOffset(2025, 2, 2, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 4,
                    CenterId = 4,
                    CarId = 4, // Sport
                    Units = 1,
                    UnitPrice = 18200m,  // con impuesto del 7%
                    TotalTax = 1274m,
                    Total = 19474m,
                    Date = new DateTimeOffset(2025, 2, 18, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 5,
                    CenterId = 1,
                    CarId = 2, // SUV
                    Units = 5,
                    UnitPrice = 9500m,
                    TotalTax = 0,
                    Total = 5 * 9500m,
                    Date = new DateTimeOffset(2025, 3, 5, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 6,
                    CenterId = 2,
                    CarId = 3, // Offroad
                    Units = 1,
                    UnitPrice = 12500m,
                    TotalTax = 0,
                    Total = 12500m,
                    Date = new DateTimeOffset(2025, 3, 20, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 7,
                    CenterId = 3,
                    CarId = 4, // Sport
                    Units = 2,
                    UnitPrice = 18200m,
                    TotalTax = 2 * 1274,
                    Total = 2 * 19474m,
                    Date = new DateTimeOffset(2025, 4, 12, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                },
                new Sale
                {
                    Id = 8,
                    CenterId = 4,
                    CarId = 1, // Sedan
                    Units = 6,
                    UnitPrice = 8000m,
                    TotalTax = 0,
                    Total = 6 * 8000m,
                    Date = new DateTimeOffset(2025, 5, 3, 0, 0, 0, TimeSpan.Zero),
                    UserName = "System"
                }
            };

            db.Sales.AddRange(sales);
            db.SaveChanges();
        }

        private static void SeedCarTaxes(ApplicationDbContext db)
        {
            if (db.CarTaxes.Any()) return;

            db.CarTaxes.AddRange(
                new CarTax { Id = 1, CarId= 4, Name= "Impuesto Sport 7%", Percentage = 7 }
            );

            db.SaveChanges();
        }
    }
}
