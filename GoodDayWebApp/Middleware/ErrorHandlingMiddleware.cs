using GoodDayWebApp.Errors;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace GoodDayWebApp.Middleware;

public class ErrorHandlingMiddleware(
  RequestDelegate next,
  ILogger<ErrorHandlingMiddleware> logger)
{
  public async Task Invoke(HttpContext context, ILocalizationResolver localizationResolver)
  {
    try
    {
      await next(context);
    }
    catch (AuthenticationException)
    {
      await Results.Unauthorized().ExecuteAsync(context);
    }
    catch (AppValidationException appValidationEx)
    {
      await HandleAppValidationAsync(context, localizationResolver, appValidationEx);
    }
    catch (BadHttpRequestException badHttpRequestEx)
    {
      await HandleBadHttpRequestAsync(context, localizationResolver, badHttpRequestEx);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, localizationResolver, ex);
    }
  }

  public Task HandleExceptionAsync(HttpContext context, ILocalizationResolver localizationResolver, Exception exception)
  {
    logger.LogError(exception, exception.Message);

    ProblemDetails problem = ProblemDetailsFactory.ServerError(context, localizationResolver);

    return Results.Problem(problem).ExecuteAsync(context);
  }

  public Task HandleBadHttpRequestAsync(HttpContext context, ILocalizationResolver localizationResolver, BadHttpRequestException exception)
  {
    logger.LogError(exception, exception.Message);

    ProblemDetails problem = ProblemDetailsFactory.BadRequest(context, localizationResolver, exception.Message);

    return Results.Problem(problem).ExecuteAsync(context);
  }

  public Task HandleAppValidationAsync(HttpContext context, ILocalizationResolver localizationResolver, AppValidationException exception)
  {
    ProblemDetails problem = ProblemDetailsFactory.BadRequest(context, localizationResolver, exception.Message, exception.ValidationResults);

    return Results.Problem(problem).ExecuteAsync(context);
  }
}
