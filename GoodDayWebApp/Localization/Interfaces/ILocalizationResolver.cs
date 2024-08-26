namespace GoodDayWebApp.Localization.Interfaces;

public interface ILocalizationResolver
{
  (string local, string english) Resolve(string key, params object[] args);
  string ResolveLocal(string key, params object[] args);
}