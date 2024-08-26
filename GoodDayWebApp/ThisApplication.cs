using GoodDayWebApp.Environments;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace GoodDayWebApp;

public class ThisApplication : IThisApplication
{
  public string Path { get; }
  public string Name { get; }
  public bool RunningInDebugMode { get; }
  public int MaxNumberOfRecordsPerDay { get; } = 3;
  public int MaxNumberOfDaysInThePast { get; } = 3;

  public Assembly ExecutingAssembly { get; }

  public Dictionary<string, List<string>> RandomGratitudeSentences { get; private set; } = null!;

  private readonly JsonSerializerOptions _jsonSerializerOptions = 
    new() { PropertyNameCaseInsensitive = true };

  public ThisApplication()
  {
#if DEBUG
    RunningInDebugMode = true;
#else
    RunningInDebugMode = false;
#endif

    var path = Assembly.GetExecutingAssembly().Location;

    Path = System.IO.Path.GetDirectoryName(path)!;
    Name = System.IO.Path.GetFileName(path)!;
    ExecutingAssembly = Assembly.GetExecutingAssembly();

    LoadGratitudeSentences();
  }

  public void OpenBrowser(IConfiguration configuration, ISupportedEnvironment supportedEnvironment)
  {
    if (!supportedEnvironment.OpenBrowser)
      return;

    try
    {
      Process.Start(new ProcessStartInfo
      {
        FileName = GetLocalhostUrl(configuration),
        UseShellExecute = true
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to open browser: {ex.Message}");
    }
  }

  private static string GetLocalhostUrl(IConfiguration configuration)
  {
    string? urls = configuration["urls"];
    if (string.IsNullOrEmpty(urls))
      urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000;";

    var url = urls
      .Split(';')
      .FirstOrDefault(u => u.StartsWith("https://") || u.StartsWith("http://"));

    return url!;
  }

  private void LoadGratitudeSentences()
  {
    string filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "gratitude_sentences.json");
    string jsonString = File.ReadAllText(filePath);
    var gratitudeData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(jsonString, _jsonSerializerOptions);
    if (gratitudeData != null && gratitudeData.TryGetValue("gratitude_sentences", out Dictionary<string, List<string>>? value))
    {
      RandomGratitudeSentences = value;
    }
  }
}
