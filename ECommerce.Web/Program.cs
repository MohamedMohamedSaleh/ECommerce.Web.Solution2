
using ECommerce.Domain.Contracts;
using ECommerce.persistence.Data.DataSeed;
using ECommerce.persistence.Data.DbContexts;
using ECommerce.persistence.Data.Repositories;
using ECommerce.ServiceAbstraction;
using ECommerce.Services;
using ECommerce.Services.MappingProfiles;
using ECommerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace ECommerce.Web
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection")!);
                return ConnectionMultiplexer.Connect(configuration);
            });

            var app = builder.Build();

            #region Data Seed

            await app.MigrateDbAsync();
            await app.seedDbAsync();


            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
