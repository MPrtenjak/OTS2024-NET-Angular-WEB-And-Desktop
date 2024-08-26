using Dapper;
using GoodDayWebApp.Database.Repositories;
using System.Data;

namespace GoodDayWebApp.Database.Interface;

public class BaseRepository(IDapperContext dapperContext) : IBaseRepository
{
  public T? ReadOne<T>(string query, object? param = null)
  {
    using IDbConnection connection = dapperContext.CreateConnection();

    return connection.Query<T>(query, param).FirstOrDefault();
  }

  public IEnumerable<T> ReadList<T>(string query, object? param = null)
  {
    using IDbConnection connection = dapperContext.CreateConnection();
    return connection.Query<T>(query, param);
  }

  public long Execute(string query, object? param = null)
  {
    using IDbConnection connection = dapperContext.CreateConnection();
    return connection.Execute(query, param);
  }

  public long Insert(string query, object? param = null)
  {
    using IDbConnection connection = dapperContext.CreateConnection();
    query += "; SELECT last_insert_rowid();";
    return connection.ExecuteScalar<long>(query, param);
  }
}
