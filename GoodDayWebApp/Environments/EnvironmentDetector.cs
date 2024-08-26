using Microsoft.Extensions.Hosting.WindowsServices;
using System.Runtime.InteropServices;

namespace GoodDayWebApp.Environments;

public static class EnvironmentDetector
{
  public static ISupportedEnvironment Detect()
  {
    if (IsRunningOnAzureWebApp())
      return new AzureEnvironment();

    if (OperatingSystem.IsWindows())
      return WindowsServiceHelpers.IsWindowsService()
        ? new WindowsAsServiceEnvironment()
        : new WindowsAsProgramEnvironment();

    if (OperatingSystem.IsLinux())
      return IsRunningAsLinuxDaemon()
        ? new LinuxAsServiceEnvironment()
        : new LinuxAsProgramEnvironment();

    throw new NotSupportedException("The current environment is not supported.");
  }

  private static bool IsRunningOnAzureWebApp() =>
       IsEnvironmentVariableSet("WEBSITE_INSTANCE_ID")
    && IsEnvironmentVariableSet("WEBSITE_HOSTNAME");

  private static bool IsRunningAsLinuxDaemon() =>
    IsEnvironmentVariableSet("DOTNET_RUNNING_IN_SERVICE");

  private static bool IsEnvironmentVariableSet(string variableName)
  {
    var variable = Environment.GetEnvironmentVariable(variableName);
    return !string.IsNullOrEmpty(variable);
  }
}
