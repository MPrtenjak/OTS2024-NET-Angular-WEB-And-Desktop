using GoodDayWebApp.Services.Interfaces;
using System.Management;

namespace GoodDayWebApp.Services;

public partial class UserIdentifierServiceFromEnvironment : IUserIdentifierService
{
  public string IdentifyUser()
  {
    return string.IsNullOrWhiteSpace(Environment.UserDomainName)
      ? Environment.UserName
      : $"{Environment.UserDomainName}\\{Environment.UserName}";
  }
}
