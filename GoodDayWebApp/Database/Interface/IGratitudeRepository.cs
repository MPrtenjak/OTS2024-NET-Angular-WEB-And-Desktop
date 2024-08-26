using GoodDayWebApp.DTO;
using GoodDayWebApp.DTO.Rest.Input;

namespace GoodDayWebApp.Database.Interface;

public interface IGratitudeRepository
{
  List<Gratitude> Add(HttpContext context, GratitudeAddDTO gratitudeAddDTO);
  List<Gratitude> AddRandomGratitudes(HttpContext context);
  public List<Gratitude> GetGratitudesForWholeMonth(HttpContext context, string month);
}