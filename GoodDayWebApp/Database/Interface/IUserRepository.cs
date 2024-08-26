using GoodDayWebApp.DTO;
using GoodDayWebApp.DTO.Rest.Input;
using GoodDayWebApp.DTO.Rest.Output;

namespace GoodDayWebApp.Database.Interface
{
  public interface IUserRepository
  {
    SignInResponseDTO AddUser(SignUpDTO signUpDTO);
    SignInResponseDTO LoginUser(SignInDTO signUpDTO);
    SignInResponseDTO LoginSingleModeUser();
    User? GetUserFromHttpContext(HttpContext context);
  }
}