using Asp.Versioning;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Responses;
using CleanArchitecture.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Api.Framework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultCorsPolicyName = "DefaultCorsPolicy";
        public static void AddJwtAuthentication(this IServiceCollection services, JwtConfig jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var endpoint = context.HttpContext.GetEndpoint();
                        var isAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

                        if (isAnonymous)
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                        throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed", HttpStatusCode.Unauthorized);
                    },

                    OnTokenValidated = async context =>
                    {
                        var endpoint = context.HttpContext.GetEndpoint();
                        var isAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

                        if (isAnonymous)
                        {
                            return;
                        }

                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                        if (claimsIdentity.Claims?.Any() != true)
                        {
                            context.Fail("This token has no claims.");
                            return;
                        }

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                        {
                            context.Fail("This token has no security stamp");
                            return;
                        }

                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        if (user == null || user.SecurityStamp != securityStamp)
                        {
                            context.Fail("Token security stamp is not valid.");
                            return;
                        }
                    },

                    OnChallenge = context =>
                    {
                        var endpoint = context.HttpContext.GetEndpoint();
                        var isAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

                        if (isAnonymous)
                        {
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }

                        if (context.AuthenticateFailure != null)
                        {
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "خطای احراز هویت", HttpStatusCode.OK, context.AuthenticateFailure, null);
                        }

                        throw new AppException(ApiResultStatusCode.UnAuthorized, "خطای احراز هویت", HttpStatusCode.Unauthorized);
                    }
                };
            });
        }


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

        public static void RegisterCors(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    if (environment.IsDevelopment())
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                    else
                    {
                        builder.WithOrigins(allowedOrigins ?? Array.Empty<string>())
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                });
            });
        }

        public static void UseConfiguredCors(this IApplicationBuilder app)
        {
            app.UseCors(DefaultCorsPolicyName);
        }

    }
}
