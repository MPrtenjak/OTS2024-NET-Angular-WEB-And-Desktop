using GoodDayWebApp.Resources.Interfaces;

namespace GoodDayWebApp.Resources;

public class AppResourceManager(IThisApplication thisApplication) : IAppResourceManager
{
  public string SaveResourceWithPartialNameIntoString(string resourceName, string? resourceFolder = null)
  {
    var fullResourceName = GetFullResourceName(resourceName, resourceFolder);
    using Stream resource = thisApplication.ExecutingAssembly.GetManifestResourceStream(fullResourceName)!;
    using var reader = new StreamReader(resource);
    return reader.ReadToEnd();
  }

  private string GetFullResourceName(string resourceName, string? resourceFolder)
  {
    return $"{GetResourcePrefix(resourceFolder)}{resourceName}";
  }

  private string GetResourcePrefix(string? resourceFolder)
  {
    var assemblyName = thisApplication.ExecutingAssembly.GetName().Name;
    return (resourceFolder is null)
      ? $"{assemblyName}.Resources."
      : $"{assemblyName}.Resources.{resourceFolder}.";
  }
}

