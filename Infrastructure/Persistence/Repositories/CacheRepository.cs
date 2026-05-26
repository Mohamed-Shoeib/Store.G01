using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase database = connection.GetDatabase();
        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var redisValue = JsonSerializer.Serialize(value);
            await database.StringSetAsync(key, redisValue, duration);
        }

        public async Task<string?> GetAsync(string key)
        {
             var value = await database.StringGetAsync(key);
             //return value.HasValue ? value.ToString() : null;
            return !value.IsNullOrEmpty ? value : default;
        }
    }
}
