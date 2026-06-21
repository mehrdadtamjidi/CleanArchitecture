using CleanArchitecture.Application.Common;
using StackExchange.Redis;

namespace CleanArchitecture.Infrastructure.Cache
{
    public static class RedisConnectionFactory
    {
        public static IConnectionMultiplexer Create(RedisSettings settings)
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ConnectTimeout = 5000,
                ReconnectRetryPolicy = new ExponentialRetry(5000)
            };

            foreach (var node in settings.Nodes)
                options.EndPoints.Add(node);

            return ConnectionMultiplexer.Connect(options);
        }
    }
}
