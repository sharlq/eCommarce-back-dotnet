using Core.models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class BasketRepo : IBasketRepo
    {
        private readonly IDatabase _database;

        public BasketRepo(ConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteBasketAsync(string basketId)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {

            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);

        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {

            var created = await _database.StringSetAsync(basket.Id,
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!created) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
