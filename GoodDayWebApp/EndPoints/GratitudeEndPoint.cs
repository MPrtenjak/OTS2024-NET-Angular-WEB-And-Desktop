using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.DTO;
using GoodDayWebApp.DTO.Rest.Input;
using Microsoft.AspNetCore.Mvc;

namespace GoodDayWebApp.EndPoints;

public static class GratitudeEndPoint
{
  public static RouteGroupBuilder MapGratitudeEndpoint(this RouteGroupBuilder group)
  {
    _ = group.MapPost("", (
      HttpContext context,
      [FromServices] IGratitudeRepository gratitudeRepository,
      [FromBody] GratitudeAddDTO gratitudeAddDTO) =>
    {
      List<Gratitude> result = gratitudeRepository.Add(context, gratitudeAddDTO);
      return Results.Ok(result);
    })
    .RequireAuthorization("UserRolePolicy")
    .WithOpenApi();

    _ = group.MapGet("{month}", (
      HttpContext context,
      [FromServices] IGratitudeRepository gratitudeRepository,
      string month) =>
    {
      List<Gratitude> result = gratitudeRepository.GetGratitudesForWholeMonth(context, month);
      return Results.Ok(result);
    })
    .RequireAuthorization("UserRolePolicy")
    .WithOpenApi();

    return group;
  }
}
