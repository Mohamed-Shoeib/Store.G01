using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Persistence
{
    public class DbInitailizer : IDbInitailizer
    {
        private readonly StoreDbContext context;
        public DbInitailizer(StoreDbContext _context)
        {
            context = _context;
        }
        public async Task InitializeAsync()
        {
            try
            {
                // Create Database and Tables If Not Exists And Apply Pending Migrations

                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync();
                }

                // Data Seeding

                // Seed ProductTypes From Json File

                if (!context.ProductTypes.Any())
                {
                    // Seed ProductTypes From Json File

                    // 1. Read All Data From Json File as String
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\types.json");

                    // 2. Transorm String To C# Object (List<ProductType>) Using JsonSerializer
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // 3. Add List<ProductType> To Database Using AddRangeAsync Method
                    if (types is not null && types.Any())
                    {
                        await context.ProductTypes.AddRangeAsync(types);
                        await context.SaveChangesAsync();
                    }
                }

                // Seed ProductBrands From Json File

                if (!context.ProductBrands.Any())
                {
                    // Seed ProductBrands From Json File

                    // 1. Read All Data From Json File as String
                    var brandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\brands.json");

                    // 2. Transorm String To C# Object (List<Productbrands>) Using JsonSerializer
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                    // 3. Add List<Productbrands> To Database Using AddRangeAsync Method
                    if (brands is not null && brands.Any())
                    {
                        await context.ProductBrands.AddRangeAsync(brands);
                        await context.SaveChangesAsync();
                    }
                }

                // Seed Products From Json File

                if (!context.Products.Any())
                {
                    // Seed Products From Json File

                    // 1. Read All Data From Json File as String
                    var productData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\Products.json");

                    // 2. Transorm String To C# Object (List<Product>) Using JsonSerializer
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    // 3. Add List<Productbrands> To Database Using AddRangeAsync Method
                    if (products is not null && products.Any())
                    {
                        await context.Products.AddRangeAsync(products);
                        await context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
