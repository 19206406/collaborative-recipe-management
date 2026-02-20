using StackExchange.Redis;
using System.Text.Json;

namespace Recommendation.API.Common.Cache
{
    public class CacheService(IConnectionMultiplexer redis) : ICacheService
    {
        private readonly IDatabase _db = redis.GetDatabase(); 

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(value!); 
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key); 
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class
        {
            var serialized = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, serialized, expiration); 
        }

    }
}
