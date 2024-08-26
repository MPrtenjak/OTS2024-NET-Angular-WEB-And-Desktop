using GoodDayWebApp.Errors;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.Validators;

public static class RestCallDTOValidator
{
  public static void Validate<DTO>(
    ILocalizationResolver localizationResolver,
    DTO dto)
  {
    var validationResults = new List<ValidationResult>();

    var isValid = false;
    if (dto is null)
    {
      var missingDataError = localizationResolver.ResolveLocal(ValidationKeys.MissingData);
      validationResults.Add(new ValidationResult(missingDataError));
    }
    else
    {
      var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
      isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);
    }

    if (isValid)
      return;

    var detail = localizationResolver.ResolveLocal(ValidationKeys.ValidationError);
    throw new AppValidationException(detail, validationResults);
  }
}
