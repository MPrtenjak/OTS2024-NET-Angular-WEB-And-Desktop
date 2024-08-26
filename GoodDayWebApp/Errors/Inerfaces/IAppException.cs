using Microsoft.AspNetCore.Mvc;

namespace GoodDayWebApp.Errors.Inerfaces;

public interface IAppException
{
  ProblemDetails ToProblemDetails(HttpContext context);
}

