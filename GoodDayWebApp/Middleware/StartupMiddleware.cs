using GoodDayWebApp.Database.Interface;

namespace GoodDayWebApp.Middleware;

public class StartupMiddleware(RequestDelegate next)
{
  private bool _isFirstRequest = true;

  public async Task Invoke(HttpContext context, IRepository repository)
  {
    if (_isFirstRequest)
    {
      _isFirstRequest = false;
      repository.CreateDatabase();
    }

    await next(context);
  }
}
