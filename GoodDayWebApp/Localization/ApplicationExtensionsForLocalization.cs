namespace GoodDayWebApp.Localization;

public static class ApplicationExtensionsForLocalization
{
  public static WebApplication UseCustomRequestLocalization(this WebApplication app)
  {
    var defaultCulture = "en-US";
    var supportedCultures = new[] { defaultCulture, "en", "si-SL", "si" };
    RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
        .SetDefaultCulture(defaultCulture)
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    _ = app.UseRequestLocalization(localizationOptions);
    return app;
  }
}
