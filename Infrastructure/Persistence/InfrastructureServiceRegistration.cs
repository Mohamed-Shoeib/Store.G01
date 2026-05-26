using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<StoreDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            service.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            // Allow DI to Dbinitializer
            service.AddScoped<IDbInitailizer, DbInitailizer>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IBasketRepository, BasketRepository>();
            service.AddScoped<ICacheRepository, CacheRepository>();
            service.AddSingleton<IConnectionMultiplexer>((serviceProvider =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            }));
            return service;
        }
    }
}
