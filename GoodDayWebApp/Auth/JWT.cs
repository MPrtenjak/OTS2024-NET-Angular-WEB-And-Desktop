using GoodDayWebApp.Auth.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoodDayWebApp.Auth
{
  public class JWT(IThisApplication thisApplication, IJWTElements jwtElements) : IJWT
  {
    public static string GenerateSalt(int size = 32)
    {
      var saltBytes = new byte[size];

      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(saltBytes);
      }

      return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt)
    {
      using var sha256 = SHA256.Create();
      var saltedPassword = string.Concat(password, salt);
      var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
      var hashBytes = SHA256.HashData(saltedPasswordBytes);
      return Convert.ToBase64String(hashBytes);
    }

    public static (string salt, string hashPassword) GenerateSaltAndHashPassword(string password)
    {
      var salt = GenerateSalt();
      var hashedPassword = HashPassword(password, salt);

      return (salt, hashedPassword);
    }

    public static bool CheckPassword(string password, string salt, string hashPassword)
    {
      var hashedPassword = HashPassword(password, salt);

      return hashPassword == hashedPassword;
    }

    public static int? GetUserIdFromHttpContext(HttpContext context)
    {
      Claim? userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");
      if (userIdClaim == null)
        return null;

      var userId = userIdClaim.Value;
      return int.Parse(userId, CultureInfo.InvariantCulture);
    }

    public string CreateToken(DTO.User user)
    {
      var expirationInHours = thisApplication.RunningInDebugMode ? 24 : 1;

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(jwtElements.Key);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(
          [
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString(CultureInfo.InvariantCulture))
          ]),
        Expires = DateTime.UtcNow.AddHours(expirationInHours),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = jwtElements.Issuer,
        Audience = jwtElements.Audience
      };

      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}
