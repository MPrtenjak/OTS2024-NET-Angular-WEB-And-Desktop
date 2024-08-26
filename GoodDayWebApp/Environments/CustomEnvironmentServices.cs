namespace GoodDayWebApp.Environments;

public static class CustomEnvironmentServices
{
  public static ConfigureHostBuilder ApplyDeamons(this ConfigureHostBuilder configureHostBuilder, ISupportedEnvironment runningEnvironment)
  {
    runningEnvironment.ApplyDeamon(configureHostBuilder);
    return configureHostBuilder;
  }
}
