using StackExchange.Redis;
using System.Text.Json;

namespace Recommendation.API.Common.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db; 
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(
            IConnectionMultiplexer redis, 
            ILogger<RedisCacheService> logger)
        {
            _redis = redis;
            _db = redis.GetDatabase(); 
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                var value = await _db.StringGetAsync(key);

                if (!value.HasValue)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                    return null;
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache for key: {Key}", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, serialized, expiration);

                _logger.LogDebug("Cache set for key: {Key}, expires in: {Expiration}",
                key, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
                _logger.LogDebug("Cache removed for key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache for key: {Key}", key);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _db.KeyExistsAsync(key); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking cache existence for key: {Key}", key);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: pattern);
                return keys.Select(k => k.ToString()); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting keys with pattern: {Pattern}", pattern);
                return Enumerable.Empty<string>();
            }
        }
    }
}
