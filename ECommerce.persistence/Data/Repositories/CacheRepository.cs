using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.persistence.Data.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        // Connection to redis and caching logic would be implemented here.
        // For example, you might use StackExchange.Redis library to interact with Redis.

        private readonly IDatabase _connection;

        public CacheRepository(IConnectionMultiplexer connection)
        {
            _connection = connection.GetDatabase();
        }

        public async Task<string?> GetAsync(string cacheKey)
        {
            var cacheValue = await _connection.StringGetAsync(cacheKey);
            return cacheValue.HasValue ? cacheValue.ToString() : null;
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive)
        {
            var jsonValue = await _connection.StringGetAsync(cacheKey);
            await _connection.StringSetAsync(cacheKey, cacheValue, timeToLive);
        }
    }
}
