using GoodDayWebApp.Auth;
using GoodDayWebApp.Auth.Interface;
using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.DTO;
using GoodDayWebApp.DTO.Rest.Input;
using GoodDayWebApp.DTO.Rest.Output;
using GoodDayWebApp.Environments;
using GoodDayWebApp.Errors;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using GoodDayWebApp.Services;
using GoodDayWebApp.Services.Interfaces;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Security.Authentication;

namespace GoodDayWebApp.Database.Repositories;

public class UserRepository(
  IDapperContext dapperContext, 
  IJWT jwt, 
  ISupportedEnvironment supportedEnvironment,
  IUserIdentifierService userIdentifierService,
  ILocalizationResolver localizationResolver)
  : BaseRepository(dapperContext), IUserRepository
{
  public SignInResponseDTO AddUser(SignUpDTO signUpDTO)
  {
    if (DoesUserExists(signUpDTO.UserName))
      ThrowSignInException(nameof(signUpDTO.UserName), ValidationKeys.UserAlreadyExists);

    User user = AddUserToDb(signUpDTO);

    return GenerateSignInResponseDTO(user);
  }

  private User AddUserToDb(SignUpDTO signUpDTO)
  {
    (var salt, var hashPassword) = JWT.GenerateSaltAndHashPassword(signUpDTO.Password1);

    var user = new User
    {
      Name = signUpDTO.UserName,
      Password = hashPassword,
      Salt = salt,
      Role = UserRole.User
    };

    InsertUser(user);
    return user;
  }

  public SignInResponseDTO LoginUser(SignInDTO signUpDTO)
  {
    User? user = ReadUserByName(signUpDTO.UserName);

    if (user == null)
      ThrowSignInException(nameof(signUpDTO.UserName), ValidationKeys.UserOrPasswordIsWrong);

    if (!JWT.CheckPassword(signUpDTO.Password, user!.Salt, user!.Password))
      ThrowSignInException(nameof(signUpDTO.UserName), ValidationKeys.UserOrPasswordIsWrong);

    return GenerateSignInResponseDTO(user);
  }

  public SignInResponseDTO LoginSingleModeUser()
  {
    if (supportedEnvironment.RequireLogin)
      throw new AuthenticationException();

    var userName = userIdentifierService.IdentifyUser();
    var (signUp, signIn) = GenerateFakeSignInAndSignUp(userName);
    var user = ReadUserByName(userName);

    if (user == null)
      AddUserToDb(signUp);

    return LoginUser(signIn);
  }

  public User? GetUserFromHttpContext(HttpContext context)
  {
    var userId = JWT.GetUserIdFromHttpContext(context);
    return ReadOne<User>("SELECT * FROM Users WHERE id = @Id", new { Id = userId });
  }

  private void ThrowSignInException(string property, string validationError)
  {
    var errorMessage = localizationResolver.ResolveLocal(validationError);
    var validationResult = new ValidationResult(errorMessage, [property]);
    throw new AppValidationException(errorMessage, [validationResult]);
  }

  private SignInResponseDTO GenerateSignInResponseDTO(User user)
  {
    var token = jwt.CreateToken(user);

    return new SignInResponseDTO()
    {
      UserId = user.Id,
      UserName = user.Name,
      Token = token
    };
  }

  private void InsertUser(User user)
  {
    var query = "INSERT INTO users(name, password, role, salt) VALUES (@name, @password, @role, @salt);";
    var parameters = new { name = user.Name, password = user.Password, role = user.Role, salt = user.Salt };
    user.Id = (int)Insert(query, parameters);
  }

  private User? ReadUserByName(string name)
  {
    var query = "SELECT * FROM Users WHERE name = @Name";
    var parameters = new { Name = name };
    return ReadOne<User>(query, parameters);
  }

  private static (SignUpDTO, SignInDTO) GenerateFakeSignInAndSignUp(string user)
  {
    var signUp = new SignUpDTO()
    {
      UserName = user,
      Password1 = ThisAppConstants.FAKE_USER_PASSWORD,
      Password2 = ThisAppConstants.FAKE_USER_PASSWORD,
    };

    var signIn = new SignInDTO()
    {
      UserName = user,
      Password = ThisAppConstants.FAKE_USER_PASSWORD,
    };

    return (signUp, signIn);
  }

  private bool DoesUserExists(string name) =>
    ReadUserByName(name) != null;
}
