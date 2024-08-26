using Microsoft.OpenApi.Models;

namespace GoodDayWebApp.EndPoints;

public static class ServiceExtensionsForCors
{
  public static IServiceCollection AddCorsSupport(this IServiceCollection services, IThisApplication thisApp)
  {
    if (!thisApp.RunningInDebugMode)
      return services;

    _= services.AddCors(options =>
    {
      options.AddDefaultPolicy(builder =>
      {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
      });
    });

    return services;
  }

  public static WebApplication UseCorsIfSupported(this WebApplication app, IThisApplication thisApp)
  {
    if (thisApp.RunningInDebugMode)
    {
      app.UseCors();
    }

    return app;
  }
}
