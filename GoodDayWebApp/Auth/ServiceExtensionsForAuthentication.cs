using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GoodDayWebApp.Auth;

public static class ServiceExtensionsForAuthentication
{
  public static IServiceCollection AddCustomAuthenticationAndAddAuthorization(this IServiceCollection services, IConfiguration configuration)
  {
    var jwtElements = new JWTElements(configuration);

    _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidIssuer = jwtElements.Issuer,
          ValidateAudience = true,
          ValidAudience = jwtElements.Audience,
          ValidateLifetime = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtElements.Key)),
          ValidateIssuerSigningKey = true
        };
      });

    _ = services.AddAuthorization(options =>
    {
      options.AddPolicy("UserRolePolicy", policy => policy.RequireRole("User"));
      options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
    });

    return services;
  }
}