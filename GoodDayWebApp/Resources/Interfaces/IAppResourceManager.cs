namespace GoodDayWebApp.Resources.Interfaces;

public interface IAppResourceManager
{
  string SaveResourceWithPartialNameIntoString(string resourceName, string? resourceFolder = null);
}
