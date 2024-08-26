using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoodDayWebApp.Localization;

public class LocalizationResolver(IStringLocalizer<ValidationErrors> localizer, IEnglishLocalization englishLocalization) : ILocalizationResolver
{
  private readonly CultureInfo _englishCulture = new("en-US");

  public (string local, string english) Resolve(string key, params object[] args)
  {
    LocalizedString local = localizer[key, args];

    var english = CultureInfo.CurrentCulture.Equals(_englishCulture)
      ? local
      : englishLocalization.Localize(key, args);

    return (local, english);
  }

  public string ResolveLocal(string key, params object[] args)
  {
    return localizer[key, args];
  }
}
