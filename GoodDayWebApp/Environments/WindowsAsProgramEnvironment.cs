namespace GoodDayWebApp.Environments;

public class WindowsAsProgramEnvironment : ISupportedEnvironment
{
  public string Name => nameof(WindowsAsProgramEnvironment);
  public bool RequireLogin { get => false; }
  public bool OpenBrowser { get => true; }
}
