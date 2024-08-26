using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.DTO;
using GoodDayWebApp.DTO.Rest.Input;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Validators;
using System.Globalization;
using System.Security.Authentication;

namespace GoodDayWebApp.Database.Repositories;

public class GratitudeRepository(
  IThisApplication thisApplication,
  IDapperContext dapperContext,
  IUserRepository userRepository,
  ILocalizationResolver localizationResolver)
  : BaseRepository(dapperContext), IGratitudeRepository
{
  private readonly static GratitudeAddDTOValidator _dtoValidator = new();
  private readonly Random _random = new();

  public List<Gratitude> Add(HttpContext context, GratitudeAddDTO gratitudeAddDTO)
  {
    RestCallDTOValidator.Validate(localizationResolver, gratitudeAddDTO);
    _dtoValidator.Validate(gratitudeAddDTO, thisApplication, localizationResolver);

    User user = userRepository.GetUserFromHttpContext(context) ?? throw new AuthenticationException();

    return InsertGratitude(gratitudeAddDTO, user);
  }

  public List<Gratitude> AddRandomGratitudes(HttpContext context)
  {
    var results = new List<Gratitude>();
    var today = DateTime.Now;
    var language = DetermineLanguage(context);
    var user = userRepository.GetUserFromHttpContext(context) ?? throw new AuthenticationException();
    List<string> gratitudes = thisApplication.RandomGratitudeSentences[language];

    for (var day = 0; day < 90; day++)
    {
      if (ShouldFillGratitude())
      {
        var gratitudesForDay = GenerateRandomGratitudesForDay(today.AddDays(-day), gratitudes);
        results.AddRange(InsertGratitude(gratitudesForDay, user));
      }
    }

    return results;
  }

  public List<Gratitude> GetGratitudesForWholeMonth(HttpContext context, string month)
  {
    // get date from string of format yyyy-MM-dd
    var date = DateTime.ParseExact(month + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

    var user = userRepository.GetUserFromHttpContext(context) ?? throw new AuthenticationException();
    var query = "SELECT * FROM Gratitude WHERE user_id = @UserId AND date >= @FirstDayOfMonth AND date <= @LastDayOfMonth ORDER BY date";
    return ReadList<Gratitude>(query, new { UserId = user.Id, FirstDayOfMonth = firstDayOfMonth, LastDayOfMonth = lastDayOfMonth }).ToList();
  }

  private bool ShouldFillGratitude() => _random.Next(0, 100) < 33;

  private GratitudeAddDTO GenerateRandomGratitudesForDay(DateTime date, List<string> gratitudes)
  {
    var numberOfGratitudes = _random.Next(1, thisApplication.MaxNumberOfRecordsPerDay + 1);
    var selectedGratitudes = gratitudes.OrderBy(x => _random.Next()).Take(numberOfGratitudes).ToArray();

    return new GratitudeAddDTO
    {
      Date = date.ToString("yyyy-MM-dd"),
      Content = selectedGratitudes
    };
  }

  private static string DetermineLanguage(HttpContext context)
  {
    var acceptedLanguage = context.Request.Headers.AcceptLanguage.ToString();
    return !string.IsNullOrEmpty(acceptedLanguage) && acceptedLanguage.StartsWith("en") ? "en" : "si";
  }

  private List<Gratitude> InsertGratitude(GratitudeAddDTO gratitudeAddDTO, User user)
  {
    var query = "DELETE FROM Gratitude WHERE user_id = @UserId AND @Date = date;";
    _ = Execute(query, new { UserId = user.Id, gratitudeAddDTO.Date });

    if (gratitudeAddDTO.Content.Length == 0)
      return [];

    foreach (var content in gratitudeAddDTO.Content)
    {
      if (string.IsNullOrWhiteSpace(content))
        continue;

      query = "INSERT INTO Gratitude (user_id, date, content) VALUES (@UserId, @Date, @Content)";
      var parameters = new { UserId = user.Id, gratitudeAddDTO.Date, Content = content };
      _ = Insert(query, parameters);
    }

    return GetGratitudePerDateAndUser(user, gratitudeAddDTO.Date);
  }

  private List<Gratitude> GetGratitudePerDateAndUser(User user, string date)
  {
    var query = "SELECT * FROM Gratitude WHERE user_id = @UserId AND date = @Date";
    return ReadList<Gratitude>(query, new { UserId = user.Id, Date = date }).ToList();
  }
}
