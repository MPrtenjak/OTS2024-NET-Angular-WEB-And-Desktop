namespace GoodDayWebApp.Environments;

public interface ISupportedEnvironment
{
  string Name { get; }
  bool RequireLogin { get => true; }
  bool OpenBrowser { get => false; }
  void ApplyDeamon(ConfigureHostBuilder configureHostBuilder) { }
}
