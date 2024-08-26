using GoodDayWebApp.DTO;

namespace GoodDayWebApp.Auth.Interface
{
  public interface IJWT
  {
    string CreateToken(User user);
  }
}