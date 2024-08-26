using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.Validators;

public static class ValidationConverter
{
  public static IEnumerable<UIValidationError> ValidationResult2UIValidationError(IEnumerable<ValidationResult> validationResults)
  {
    var fields = new Dictionary<string, List<string>>();

    foreach (ValidationResult validationResult in validationResults)
    {
      // memberNames can be Empty!
      List<string> memberNames = (validationResult.MemberNames.Any())
        ? validationResult.MemberNames.ToList()
        : [""];

      foreach (var memberName in memberNames)
      {
        if (!fields.TryGetValue(memberName, out List<string>? value))
        {
          value = [];
          fields[memberName] = value;
        }

        value.Add(validationResult.ErrorMessage ?? string.Empty);
      }
    }

    return fields.Select(x => new UIValidationError(x.Key, x.Value));
  }

  public static IDictionary<string, object?> ValidationResult2StringDictionary(IEnumerable<ValidationResult> validationResults, string mainTag = "UI")
  {
    IEnumerable<UIValidationError> result = ValidationResult2UIValidationError(validationResults);

    var castedDictionary = new Dictionary<string, object?>();
    foreach (UIValidationError entry in result)
    {
      castedDictionary[entry.Field] = entry.Messages;
    }

    return new Dictionary<string, object?>() { { mainTag, castedDictionary } };
  }
}
