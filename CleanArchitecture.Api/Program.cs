using CleanArchitecture.Api.Framework.Configuration;
using CleanArchitecture.Api.Framework.Middlewares;
using CleanArchitecture.Api.Framework.Swagger;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


#region Add Site Settings
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
#endregion
#region Register Swagger
builder.Services.AddSwagger();
#endregion
#region Register ApiVersion
builder.Services.AddCustomApiVersioning();
#endregion
#region Register Cors
builder.Services.RegisterCors(builder.Configuration, builder.Environment);
#endregion

#region Register Application Layer
builder.Services.ConfigureApplicationServices();
#endregion

#region Register Application Layer
builder.Services.ConfigureApplicationServices();
#endregion

#region Register Infrastracture Layer
builder.Services.ConfigureInfrastractureServices(builder.Configuration);
#endregion

#region Register Persistence Layer
builder.Services.ConfigurePersistenceServices(builder.Configuration);
#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region SwaggerAndUI
//app.UseSwaggerAndUI();
#endregion


app.UseTrace();

app.UseHttpsRedirection();

app.UseGlobalExceptionMiddleware();

app.UseAuthentication();

app.UseAuthorization();

#region Use CORS
app.UseConfiguredCors();
#endregion

app.MapControllers();

app.Run();
