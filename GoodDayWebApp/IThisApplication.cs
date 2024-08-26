using GoodDayWebApp.Environments;
using System.Reflection;

namespace GoodDayWebApp;

public interface IThisApplication
{
  string Name { get; }
  string Path { get; }
  bool RunningInDebugMode { get; }
  int MaxNumberOfRecordsPerDay { get; }
  int MaxNumberOfDaysInThePast { get; }
  Dictionary<string, List<string>> RandomGratitudeSentences { get; }
  Assembly ExecutingAssembly { get; }
  void OpenBrowser(IConfiguration configuration, ISupportedEnvironment supportedEnvironment);
}