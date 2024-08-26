using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoodDayWebApp.Localization;

public class EnglishLocalization(IStringLocalizerFactory localizationFactory) : IEnglishLocalization
{
  private readonly CultureInfo _englishCulture = new("en-US");
  private readonly IStringLocalizer _englishLocalizer = localizationFactory.Create(typeof(ValidationErrors));

  public string Localize(string key, params object[] args)
  {
    CultureInfo originalUICulture = CultureInfo.CurrentUICulture;

    try
    {
      CultureInfo.CurrentUICulture = _englishCulture;
      return _englishLocalizer[key, args];
    }
    finally
    {
      CultureInfo.CurrentUICulture = originalUICulture;
    }
  }
}

