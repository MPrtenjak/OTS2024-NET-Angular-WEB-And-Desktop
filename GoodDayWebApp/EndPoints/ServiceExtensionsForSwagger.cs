using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Web.Services.Description;

namespace GoodDayWebApp.EndPoints;

public static class ServiceExtensionsForSwagger
{
  public static IServiceCollection AddSwaggerSupport(this IServiceCollection services, IThisApplication thisApp)
  {
    /*
    if (!thisApp.RunningInDebugMode)
      return services;
    */

    var bearerSecurityScheme = new OpenApiSecurityScheme
    {
      Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.ApiKey,
      Scheme = "Bearer",
      BearerFormat = "JWT",
    };

    var oauth2SecurityScheme = new OpenApiSecurityScheme
    {
      Reference = new OpenApiReference
      {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
      },
      Scheme = "oauth2",
      Name = "Bearer",
      In = ParameterLocation.Header,
    };

    var securityRequirement = new OpenApiSecurityRequirement()
      { { oauth2SecurityScheme, new List<string>() } };

    _ = services.AddEndpointsApiExplorer();
    _ = services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new() { Title = "Good Day", Version = "v1" });
      c.AddSecurityDefinition("Bearer", bearerSecurityScheme);
      c.AddSecurityRequirement(securityRequirement);
    });

    return services;
  }

  public static WebApplication UseSwaggerIfSupported(this WebApplication app, IThisApplication thisApp)
  {
    
    if (true /*thisApp.RunningInDebugMode*/)
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    return app;
  }
}
