using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.Auth.CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.HttpRequest;
using CleanArchitecture.Infrastructure.Mail;
using CleanArchitecture.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
