using GoodDayWebApp.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.DTO.Rest.Input;

public record GratitudeAddDTO
{
  [Required(
    ErrorMessageResourceName = ValidationKeys.GratitudeDateRequired, ErrorMessageResourceType = typeof(ValidationErrors))]
  public string Date { get; init; } = null!;

  [Required(
    ErrorMessageResourceName = ValidationKeys.GratitudeContentRequired, ErrorMessageResourceType = typeof(ValidationErrors))]
  public string[] Content { get; init; } = null!;

  [System.Text.Json.Serialization.JsonIgnore]
  public DateOnly? RealDate
  {
    get
    {
      if (DateOnly.TryParseExact(Date, "yyyy-MM-dd", out DateOnly date))
        return date;

      return null;
    }
  }
}