namespace GoodDayWebApp.Environments;

public class LinuxAsProgramEnvironment : ISupportedEnvironment
{
  public string Name => nameof(LinuxAsProgramEnvironment);
  public bool RequireLogin { get => false; }
}
