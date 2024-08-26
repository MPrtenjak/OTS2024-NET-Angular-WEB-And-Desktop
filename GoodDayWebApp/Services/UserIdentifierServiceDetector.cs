using GoodDayWebApp.Environments;
using GoodDayWebApp.Services.Interfaces;
using Microsoft.Extensions.Hosting.WindowsServices;

namespace GoodDayWebApp.Services;

public static class UserIdentifierServiceDetector
{
  public static IUserIdentifierService Detect()
  {
    return (WindowsServiceHelpers.IsWindowsService())
      ? new UserIdentifierServiceOnWindowsService()
      : new UserIdentifierServiceFromEnvironment();
  }
}
