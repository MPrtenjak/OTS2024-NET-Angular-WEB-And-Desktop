using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.DTO.Rest.Input;
using GoodDayWebApp.DTO.Rest.Output;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GoodDayWebApp.EndPoints;

public static class UsersEndPoint
{
  public static RouteGroupBuilder MapUsersEndpoint(this RouteGroupBuilder group)
  {
    _ = group.MapPost("/sign-up", (
      HttpContext context,
      [FromServices] IUserRepository userRepository,
      [FromServices] ILocalizationResolver localizationResolver,
      [FromBody] SignUpDTO signUpDTO) =>
    {
      RestCallDTOValidator.Validate(localizationResolver, signUpDTO);

      SignInResponseDTO result = userRepository.AddUser(signUpDTO);

      return Results.Ok(result);
    })
    .WithOpenApi();

    _ = group.MapPost("/login", (
      HttpContext context,
      [FromServices] IUserRepository userRepository,
      [FromServices] ILocalizationResolver localizationResolver,
      [FromBody] SignInDTO signInDTO) =>
    {
      RestCallDTOValidator.Validate(localizationResolver, signInDTO);

      SignInResponseDTO result = userRepository.LoginUser(signInDTO);

      return Results.Ok(result);
    })
    .WithOpenApi();

    _ = group.MapPost("/try-login", (
      HttpContext context,
      [FromServices] IUserRepository userRepository,
      [FromServices] ILocalizationResolver localizationResolver) =>
    {
      SignInResponseDTO result = userRepository.LoginSingleModeUser();

      return Results.Ok(result);
    })
    .WithOpenApi();

    return group;
  }
}
