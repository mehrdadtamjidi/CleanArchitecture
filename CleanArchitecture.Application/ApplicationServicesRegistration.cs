using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.CustomMapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application
{
    public static class ApplicationServicesRegistration 
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // FluentValidation: Register all validators in this assembly
            services.InitializeAutoMapper();

            // MediatR: Register request handlers
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // MediatR Pipeline: Add validation behavior globally
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
