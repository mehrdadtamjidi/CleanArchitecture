using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.Auth.CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
