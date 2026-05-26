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
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorsModel;
using Store.G02.Api.Extensions;
namespace Store.G02.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
