using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.DTO;
using GoodDayWebApp.Environments;
using GoodDayWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodDayWebApp.EndPoints;

public static class AdminEndPoint
{
  /*
   *  This end-point should not bi in a real application.
   * 
   *  We have it here so it is easier to show what is happening in the application
   */
  public static RouteGroupBuilder MapAdminEndpoint(this RouteGroupBuilder group)
  {
    _ = group.MapGet("/info", (
      HttpContext context,
      [FromServices] IThisApplication thisApplication,
      [FromServices] ISupportedEnvironment supportedEnvironment,
      [FromServices] IConfigurationStringBuilder configurationStringBuilder,
      [FromServices] IConfiguration configuration,
      [FromServices] IUserIdentifierService userIdentifierService) =>
    {
      var configurationDictionary = configuration.AsEnumerable().ToDictionary(k => k.Key, v => v.Value);
      var user = userIdentifierService.IdentifyUser();

      dynamic info = new
      {
        thisApplication.Name,
        thisApplication.Path,
        thisApplication.RunningInDebugMode,
        CurrentEnvironment = supportedEnvironment.Name,
        ExecutingAssembly = thisApplication.ExecutingAssembly.ToString(),
        configurationStringBuilder.ConnectionString,
        Configuration = configurationDictionary,
        user
      };

      return Results.Ok(info);
    });

    _ = group.MapPost("randomize-data", (
      HttpContext context,
      [FromServices] IGratitudeRepository gratitudeRepository) =>
    {
      List<Gratitude> result = gratitudeRepository.AddRandomGratitudes(context);
      return Results.Ok(result);
    })
    .RequireAuthorization("UserRolePolicy")
    .WithOpenApi();

    return group;
  }
}
