using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
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
        private readonly StoreIdentityDbContext identityDbContext;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitailizer(StoreDbContext _context,StoreIdentityDbContext _identityDbContext,UserManager<AppUser> _userManager,RoleManager<IdentityRole> _roleManager)
        { 
            context = _context;
            identityDbContext = _identityDbContext;
            userManager = _userManager;
            roleManager = _roleManager;
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

                // Seed DeliveryMethods From Json File

                if (!context.DeliveryMethods.Any())
                {
                    // Seed DeliveryMethods From Json File
                    // 1. Read All Data From Json File as String
                    var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\delivery.json");

                    // 2. Transorm String To C# Object (List<DeliveryMethod>) Using JsonSerializer
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                    // 3. Add List<DeliveryMethod> To Database Using AddRangeAsync Method
                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                        await context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // Create Database and Tables If Not Exists And Apply Pending Migrations
            if (identityDbContext.Database.GetPendingMigrations().Any())
            {
                await identityDbContext.Database.MigrateAsync();
            }

            // Roles Seeding
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                //await roleManager.CreateAsync(new IdentityRole() { Name = "Customer" });
            }

            // Data Seeding
            if (!identityDbContext.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "Superadmin@gmail.com",
                    PhoneNumber = "01000000000",
                    UserName = "superadmin"
                }; var AdminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "01132359779",
                    UserName = "admin"
                };
                await userManager.CreateAsync(superAdminUser, "Superadmin@123#");
                await userManager.CreateAsync(AdminUser, "Admin@123#");

                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await userManager.AddToRoleAsync(AdminUser, "Admin");
            }
        }
    }
}
