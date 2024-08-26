namespace GoodDayWebApp.Environments;

public class WindowsAsServiceEnvironment : ISupportedEnvironment
{
  public string Name => nameof(WindowsAsServiceEnvironment);
  public bool RequireLogin { get => false; }
  public void ApplyDeamon(ConfigureHostBuilder configureHostBuilder)
    => configureHostBuilder.UseWindowsService();
}
