using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.Errors;

public class AppValidationException : Exception
{
  public IEnumerable<ValidationResult> ValidationResults { get; }

  public AppValidationException(string detail, IEnumerable<ValidationResult> validationResults) :
    base(detail)
  {
    ValidationResults = validationResults;
  }

  public AppValidationException(string detail, Exception innerException, IEnumerable<ValidationResult> validationResults) :
    base(detail, innerException)
  {
    ValidationResults = validationResults;
  }
}