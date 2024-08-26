using System.Data;

namespace GoodDayWebApp.Database.Interface
{
  public interface IDapperContext
  {
    string ConnectionString { get; }

    IDbConnection CreateConnection();
  }
}