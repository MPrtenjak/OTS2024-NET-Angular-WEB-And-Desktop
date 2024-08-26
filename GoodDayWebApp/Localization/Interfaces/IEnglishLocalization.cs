namespace GoodDayWebApp.Localization.Interfaces
{
  public interface IEnglishLocalization
  {
    string Localize(string key, params object[] args);
  }
}