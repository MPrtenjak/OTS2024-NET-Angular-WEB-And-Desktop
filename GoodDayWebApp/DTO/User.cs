using GoodDayWebApp.Auth;

namespace GoodDayWebApp.DTO
{
  public class User
  {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public UserRole Role { get; set; }
  }
}
