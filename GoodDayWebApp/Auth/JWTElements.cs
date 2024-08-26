using GoodDayWebApp.Auth.Interface;

namespace GoodDayWebApp.Auth;

public record JWTElements(string Key, string Issuer, string Audience) : IJWTElements
{
  public JWTElements(IConfiguration configuration) :
    this(default!, configuration["Jwt:Issuer"] ?? string.Empty, configuration["Jwt:Audience"] ?? string.Empty)
  {
    var jwtKeySetting = "Jwt:Key";

    Key = configuration[jwtKeySetting]
      ?? throw new ArgumentNullException(null, nameof(jwtKeySetting));
  }
}
