using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.persistence.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        // this will connect from Radis => IConnectionMultiplexer 
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            // Register the connection multiplexer in the dependency injection container[program.cs]
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);
            if (IsCreatedOrUpdated)
            {
                var JBasket = await _database.StringGetAsync(basket.Id);
                return JsonSerializer.Deserialize<CustomerBasket>(JBasket!);

            }
            return null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var Basket = await _database.StringGetAsync(basketId);
            if (Basket.IsNullOrEmpty) return null;
            else return JsonSerializer.Deserialize<CustomerBasket>(Basket!);

        }
    }
}
