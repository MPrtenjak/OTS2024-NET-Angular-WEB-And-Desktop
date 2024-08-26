using GoodDayWebApp.Localization.Interfaces;

namespace GoodDayWebApp.Localization;

public static class ServiceExtensionsForLocalization
{
  public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
  {
    _ = services.AddLocalization(options => options.ResourcesPath = "");

    _ = services.AddScoped<ILocalizationResolver, LocalizationResolver>();
    _ = services.AddScoped<IEnglishLocalization, EnglishLocalization>();

    return services;
  }
}
