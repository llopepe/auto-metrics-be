using AutoMetricsService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests.Helpers
{
    public static class TestDataSeeder
    {
        public static List<Car> GetCars(int count = 5)
        {
            var list = new List<Car>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Car
                {
                    Id = i,
                    Name = $"Car {i}",
                    Price = 1000m + i * 100,
                    Taxes = new List<CarTax>(),
                    Sales = new List<Sale>()
                });
            }
            return list;
        }

        public static List<CarTax> GetCarTaxes(List<Car> cars)
        {
            var list = new List<CarTax>();
            int id = 1;
            foreach (var car in cars)
            {
                list.Add(new CarTax
                {
                    Id = id++,
                    Car = car,
                    CarId = car.Id,
                    Name = "IVA",
                    Percentage = 21
                });
            }
            return list;
        }

        public static List<Center> GetCenters(int count = 3)
        {
            var list = new List<Center>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Center
                {
                    Id = i,
                    Name = $"Center {i}",
                    Sales = new List<Sale>()
                });
            }
            return list;
        }

        public static List<Sale> GetSales(List<Car> cars, List<Center> centers)
        {
            var list = new List<Sale>();
            int id = 1;
            foreach (var car in cars)
            {
                foreach (var center in centers)
                {
                    var sale = new Sale
                    {
                        Id = id++,
                        Car = car,
                        CarId = car.Id,
                        Center = center,
                        CenterId = center.Id,
                        Units = 2,
                        UnitPrice = car.Price,
                        TotalTax = car.Price * 0.21m,
                        Total = car.Price + car.Price * 0.21m,
                        Date = DateTimeOffset.Now,
                        UserName = "TestUser"
                    };
                    car.Sales.Add(sale);
                    center.Sales.Add(sale);
                    list.Add(sale);
                }
            }
            return list;
        }

        public static List<User> GetUsers(int count = 3)
        {
            var list = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new User
                {
                    Id = i,
                    FullName = $"User {i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = $"hash{i}",
                    IsActive = true,
                    Roles = "User"
                });
            }
            return list;
        }
    }
}
