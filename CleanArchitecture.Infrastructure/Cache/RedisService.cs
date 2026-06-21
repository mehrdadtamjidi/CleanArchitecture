using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Cache
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;
        private readonly string _instanceName;
        private readonly TimeSpan _defaultExpiration;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public RedisService(IConnectionMultiplexer connection, IOptions<SiteSettings> siteSettings)
        {
            _db = connection.GetDatabase();
            _instanceName = siteSettings.Value.Redis.InstanceName;
            _defaultExpiration = TimeSpan.FromMinutes(siteSettings.Value.Redis.DefaultExpirationMinutes);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(BuildKey(key));

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!, _jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await _db.StringSetAsync(BuildKey(key), json, expiration ?? _defaultExpiration);
        }

        public Task RemoveAsync(string key)
            => _db.KeyDeleteAsync(BuildKey(key));

        public Task<bool> ExistsAsync(string key)
            => _db.KeyExistsAsync(BuildKey(key));

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            var cached = await GetAsync<T>(key);

            if (cached is not null)
                return cached;

            var value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }

        private string BuildKey(string key) => $"{_instanceName}:{key}";
    }
}
