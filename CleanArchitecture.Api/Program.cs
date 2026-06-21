using CleanArchitecture.Api.Framework.Configuration;
using CleanArchitecture.Api.Framework.Filters;
using CleanArchitecture.Api.Framework.Middlewares;
using CleanArchitecture.Api.Framework.Swagger;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region Add Site Settings
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
var siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
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

#region Jwt Service
builder.Services.AddJwtAuthentication(siteSetting.JwtConfig);
#endregion

#region Register Application Layer
builder.Services.ConfigureApplicationServices();
#endregion

#region Register Infrastructure Layer
builder.Services.ConfigureInfrastractureServices(builder.Configuration);
#endregion

#region Register Persistence Layer
builder.Services.ConfigurePersistenceServices(builder.Configuration);
#endregion

#region Add Trace Header Result
builder.Services.AddScoped<AddTraceHeaderResultFilter>();
#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}

#region SwaggerAndUI
app.UseSwaggerAndUI();
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
