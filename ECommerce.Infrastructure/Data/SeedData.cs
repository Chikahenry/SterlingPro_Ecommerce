using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ECommerceDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Laptop",
                    Description = "High performance laptop with 16GB RAM",
                    Price = 999.99m,
                    StockQuantity = 10,
                    ImageUrl = "https://example.com/laptop.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartphone",
                    Description = "Latest smartphone with 5G support",
                    Price = 699.99m,
                    StockQuantity = 15,
                    ImageUrl = "https://example.com/phone.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Headphones",
                    Description = "Noise cancelling wireless headphones",
                    Price = 199.99m,
                    StockQuantity = 20,
                    ImageUrl = "https://example.com/headphones.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartphone",
                    Description = "Latest smartphone with 5G support",
                    Price = 699.99m,
                    StockQuantity = 15,
                    ImageUrl = "https://example.com/phone.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Headphones",
                    Description = "Noise cancelling wireless headphones",
                    Price = 199.99m,
                    StockQuantity = 20,
                    ImageUrl = "https://example.com/headphones.jpg"
                }
            };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher();
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    PasswordHash = hasher.HashPassword("admin123")
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
