using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Services;
using AutoMapper;
using Services.Abstractions;
using Services.MappingProfiles;
using System.Threading.Tasks;
using AssemblyMapping = Services.AssemblyRefernces;
namespace Store.G02.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Allow DI to Dbinitializer
            builder.Services.AddScoped<IDbInitailizer, DbInitailizer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddAutoMapper(typeof(AssemblyMapping).Assembly);
            var app = builder.Build();

            #region Seeding
            using var scope = app.Services.CreateScope();
            // ASK CLR TO Create Object From Dbinitializer
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitailizer>();
            await dbInitializer.InitializeAsync();
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
