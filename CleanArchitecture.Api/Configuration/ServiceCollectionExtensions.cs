using Asp.Versioning;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Persistence.Repositories;

namespace CleanArchitecture.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                //url segment => {version}
                options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
                options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
                options.ReportApiVersions = true;
            });
        }

        public static void RegisterCors(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("CORSE", corsPolicyBuilder =>
                corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));
        }

    }
}
