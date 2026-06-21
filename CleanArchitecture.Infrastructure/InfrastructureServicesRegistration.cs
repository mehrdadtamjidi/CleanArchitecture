using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.Cache;
using CleanArchitecture.Infrastructure.HttpRequest;
using CleanArchitecture.Infrastructure.Mail;
using CleanArchitecture.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CleanArchitecture.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddHttpClient<IHttpService, HttpService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<SiteSettings>>().Value;
                return RedisConnectionFactory.Create(settings.Redis);
            });
            services.AddSingleton<IRedisService, RedisService>();

            return services;
        }
    }
}
