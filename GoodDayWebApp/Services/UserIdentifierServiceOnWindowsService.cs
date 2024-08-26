using GoodDayWebApp.Services.Interfaces;
using System.Management;

namespace GoodDayWebApp.Services;

public partial class UserIdentifierServiceOnWindowsService : IUserIdentifierService
{
  public string IdentifyUser()
  {
#pragma warning disable CA1416 // disable "only valid on Windows" warning

    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name = 'explorer.exe'"))
    {
      foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
      {
        var argList = new string[] { string.Empty, string.Empty };
        var returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
        if (returnVal == 0)
        {
          return string.IsNullOrEmpty(argList[1])
            ? argList[0]
            : $"{argList[1]}\\{argList[0]}";
        }
      }
    }

#pragma warning restore CA1416

    return string.Empty;
  }
}
