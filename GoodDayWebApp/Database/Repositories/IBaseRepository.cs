namespace GoodDayWebApp.Database.Repositories
{
  public interface IBaseRepository
  {
    long Execute(string query, object? param = null);
    long Insert(string query, object? param = null);
    IEnumerable<T> ReadList<T>(string query, object? param = null);
    T? ReadOne<T>(string query, object? param = null);
  }
}