﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FilterDescriptor = Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor;

namespace CleanArchitecture.Api.Framework.Swagger
{
    public class UnauthorizedResponsesOperationFilter : IOperationFilter
    {
        private readonly bool includeUnauthorizedAndForbiddenResponses;
        private readonly string schemeName;

        public UnauthorizedResponsesOperationFilter(bool includeUnauthorizedAndForbiddenResponses, string schemeName = "Bearer")
        {
            this.includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
            this.schemeName = schemeName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var metadta = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var hasAnonymous = filters.Any(p => p.Filter is AllowAnonymousFilter) || metadta.Any(p => p is AllowAnonymousAttribute);
            if (hasAnonymous) return;

            var hasAuthorize = filters.Any(p => p.Filter is AuthorizeFilter) || metadta.Any(p => p is AuthorizeAttribute);
            if (!hasAuthorize) return;

            if (includeUnauthorizedAndForbiddenResponses)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Scheme = schemeName,
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>() //new[] { "readAccess", "writeAccess" }
                }
            });
        }
    }
}
