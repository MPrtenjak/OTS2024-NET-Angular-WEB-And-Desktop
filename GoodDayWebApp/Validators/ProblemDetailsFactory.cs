using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.Validators
{
  public static class ProblemDetailsFactory
  {
    public static ProblemDetails Empty()
    {
      return new ProblemDetails();
    }

    public static ProblemDetails ServerError(
      HttpContext context,
      ILocalizationResolver localizationResolver,
      string? detail = null)
    {
      return new ProblemDetails
      {
        Title = localizationResolver.ResolveLocal(ValidationKeys.InternalServerError),
        Detail = detail,
        Instance = context.Request.Path,
        Status = StatusCodes.Status500InternalServerError,
        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
      };
    }

    public static ProblemDetails BadRequest(
      HttpContext context,
      ILocalizationResolver localizationResolver,
      string detail,
      IEnumerable<ValidationResult> validationResults)
    {
      IDictionary<string, object?> extensions = ValidationConverter.ValidationResult2StringDictionary(validationResults);

      return BadRequest(context, localizationResolver, detail, extensions);
    }

    public static ProblemDetails BadRequest(
      HttpContext context,
      ILocalizationResolver localizationResolver,
      string? detail = null,
      IDictionary<string, object?>? extensions = null)
    {
      var problemDetails = new ProblemDetails
      {
        Title = localizationResolver.ResolveLocal(ValidationKeys.BadRequest),
        Detail = detail,
        Instance = context.Request.Path,
        Status = StatusCodes.Status400BadRequest,
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      };

      if (extensions is not null)
        problemDetails.Extensions = extensions;

      return problemDetails;
    }
  }
}
