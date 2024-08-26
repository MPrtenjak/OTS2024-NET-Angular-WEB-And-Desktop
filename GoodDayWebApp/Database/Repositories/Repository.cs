using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.Resources.Interfaces;

namespace GoodDayWebApp.Database.Repositories;

public class Repository(IAppResourceManager appResourceManager, IDapperContext dapperContext)
  : BaseRepository(dapperContext), IRepository
{
  public void CreateDatabase()
  {
    var createSql = appResourceManager.SaveResourceWithPartialNameIntoString("Create_SQLite.sql", "Database");

    _ = Execute(createSql);
  }
}
