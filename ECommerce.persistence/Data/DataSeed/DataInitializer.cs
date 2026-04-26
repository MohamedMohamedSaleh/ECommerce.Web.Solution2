using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();

                if (HasProducts && HasBrands && HasTypes) return;
                Console.WriteLine(
                    "Start seeding"
                    );

                // carefully when add data 
                // organize it 

                if(!HasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType, int> ("types.json", _dbContext.ProductTypes);
                _dbContext.SaveChanges();// because listen when add products becase it depend on type and brand
                if (!HasTypes)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                _dbContext.SaveChanges();
                Console.WriteLine(
               "Success seeding################################"
               );




            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Faild Seeding {ex}");
            }

        }
        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
          
            // file path
            // E:\backend_course\ECommerce.Web.Solution2\ECommerce.persistence\Data\DataSeed\JsonFiles\
            var FilePath = @"..\ECommerce.persistence\Data\DataSeed\JsonFiles\" + fileName;
            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {fileName} Not Found!");

            try
            {
                using var DataStream = File.OpenRead(FilePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
 
                if (Data != null)
                {
                   await dbset.AddRangeAsync(Data); // 🔥 AddRange not Add
                }
            } 
            catch (Exception ex) {
                Console.WriteLine($"There are exception in SeedData: {ex}");
                throw new Exception(
                    $"There are exception in SeedData: {ex}");
            }




        }

    }
}
