using GoodDayWebApp.DTO.Rest.Input;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;

namespace GoodDayWebApp.Validators;

public class GratitudeAddDTOValidator
{
  public void Validate(GratitudeAddDTO dtoObject, IThisApplication thisApplication, ILocalizationResolver localizationResolver)
  {
    var resultBag = new ValidationResultBag(localizationResolver);
    var property = nameof(dtoObject.Date);

    if (dtoObject.RealDate == null)
      resultBag.AddValidationResult(property, ValidationKeys.DateFormatWrong);

    var today = DateOnly.FromDateTime(DateTime.Now);
    if (dtoObject.RealDate > today || dtoObject.RealDate < today.AddDays(-1 * thisApplication.MaxNumberOfDaysInThePast))
      resultBag.AddValidationResult(property, ValidationKeys.GratitudeDateOutOfRange);

    property = nameof(dtoObject.Content);
    var realContent = dtoObject.Content.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();

    // 0 is OK if we want to delete everything
    if (realContent.Length > thisApplication.MaxNumberOfRecordsPerDay)
      resultBag.AddValidationResult(property, ValidationKeys.GratitudeNumberTooHigh, "", dtoObject.Date);

    resultBag.ThrowIfInvalid();
  }
}
