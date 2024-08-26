using GoodDayWebApp.Errors;
using GoodDayWebApp.Localization.Interfaces;
using GoodDayWebApp.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.Validators;

public class ValidationResultBag(ILocalizationResolver localizationResolver)
{
  private readonly List<ValidationResult> _validationResults = [];

  public bool IsValid => _validationResults.Count == 0;

  public void AddValidationResult(string propertyName, string errorMessage, params object[] args)
  {
    var error = localizationResolver.ResolveLocal(errorMessage, args);
    _validationResults.Add(new ValidationResult(error, [propertyName]));
  }

  public void Throw()
  {
    var detail = localizationResolver.ResolveLocal(ValidationKeys.ValidationError);
    throw new AppValidationException(detail, _validationResults);
  }
  public void ThrowIfInvalid()
  {
    if (!IsValid)
      Throw();
  }
}
