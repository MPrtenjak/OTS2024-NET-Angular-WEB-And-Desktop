namespace GoodDayWebApp.Environments;

public class LinuxAsServiceEnvironment : ISupportedEnvironment
{
  public string Name => nameof(LinuxAsServiceEnvironment);
  public void ApplyDeamon(ConfigureHostBuilder configureHostBuilder) 
    => configureHostBuilder.UseSystemd();
}
