using Domain.Contracts;
using Domain.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase database = connection.GetDatabase();

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var redisValue = await database.StringGetAsync(id);
            if (redisValue.IsNullOrEmpty)
            {
                return null;
            }
            var basket = JsonSerializer.Deserialize<CustomerBasket>(redisValue);
            if (basket is null)
            {
                return null;
            }
            return basket;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLine = null)
        {
            var redisValue = JsonSerializer.Serialize(basket);
            var flag = await database.StringSetAsync(basket.Id, redisValue, TimeSpan.FromDays(30));
            return flag ? await GetBasketAsync(basket.Id) : null;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
           return await database.KeyDeleteAsync(id);
        }
    }
}
