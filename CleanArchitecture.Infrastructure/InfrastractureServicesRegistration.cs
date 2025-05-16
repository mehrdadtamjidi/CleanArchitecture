using CleanArchitecture.Application.Contracts.Infrastructure;
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
    public static class InfrastractureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastractureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
