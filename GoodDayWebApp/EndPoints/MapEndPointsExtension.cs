namespace GoodDayWebApp.EndPoints;

public static class MapEndPointsExtension
{
  public static WebApplication MapEndPoints(this WebApplication app)
  {
    app.MapGroup("/users")
       .MapUsersEndpoint()
       .WithTags("Users");

    app.MapGroup("/gratitude")
       .MapGratitudeEndpoint()
       .WithTags("Gratitude");

    app.MapGroup("/admin")
       .MapAdminEndpoint()
       .WithTags("Admin");

    return app;
  }
}
