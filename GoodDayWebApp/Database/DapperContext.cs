using GoodDayWebApp.Database.Interface;
using System.Data;
using System.Data.SQLite;

namespace GoodDayWebApp.Database;

public class DapperContext(IConfigurationStringBuilder configurationStringBuilder) : IDapperContext
{
  public string ConnectionString => configurationStringBuilder.ConnectionString;

  public IDbConnection CreateConnection()
  {
    return new SQLiteConnection(ConnectionString);
  }
}
