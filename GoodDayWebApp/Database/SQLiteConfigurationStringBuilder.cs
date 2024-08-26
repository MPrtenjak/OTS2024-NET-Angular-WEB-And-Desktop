using GoodDayWebApp.Database.Interface;

namespace GoodDayWebApp.Database;

public class SQLiteConfigurationStringBuilder : IConfigurationStringBuilder
{
  private readonly IConfiguration _configuration;
  private readonly IThisApplication _thisApplication;

  public string ConnectionString { get; }

  public SQLiteConfigurationStringBuilder(IConfiguration configuration, IThisApplication thisApplication)
  {
    _configuration = configuration;
    _thisApplication = thisApplication;

    ConnectionString = GetConnectionString();
  }

  public string GetConnectionString()
  {
    var connectionString = GetConnectionStringFromConfiguration() ?? CreateConnectionString();

    if (!connectionString.Contains(EnvTemplate))
      return connectionString;

    var dbPath = GetDatabasePath();

    if (!Directory.Exists(dbPath))
      _ = Directory.CreateDirectory(dbPath);

    return connectionString.Replace(EnvTemplate, dbPath);
  }

  public string EnvTemplate => $"%{ThisAppConstants.DATABASE_LOCATION_ENVIRONMENT_VARIABLE}%";

  public string GetConnectionStringFromConfiguration() =>
    _configuration.GetConnectionString("SQLiteConnectionString")!;

  public string CreateConnectionString() =>
    $"Data Source={EnvTemplate}\\GoodDay.db;";

  public (bool Success, string Path) GetDBPathFromEnvironmentVariable()
  {
    var dbPath = Environment.GetEnvironmentVariable(ThisAppConstants.DATABASE_LOCATION_ENVIRONMENT_VARIABLE);

    if (string.IsNullOrWhiteSpace(dbPath))
      return (false, string.Empty);

    return Directory.Exists(dbPath)
      ? (true, dbPath)
      : (false, string.Empty);
  }

  public string GetAppPath() =>
    _thisApplication.Path;

  public string GetDatabasePath()
  {
    (var envSuccess, var envPath) = GetDBPathFromEnvironmentVariable();
    var basePath = envSuccess ? envPath : GetAppPath();

    return Path.Combine(basePath, "Db");
  }
}