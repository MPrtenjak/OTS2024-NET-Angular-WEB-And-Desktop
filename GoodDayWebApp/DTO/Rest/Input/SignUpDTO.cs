using GoodDayWebApp.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace GoodDayWebApp.DTO.Rest.Input;

public record SignUpDTO
{
  [Required(
    ErrorMessageResourceName = ValidationKeys.UserNameRequired, ErrorMessageResourceType = typeof(ValidationErrors))]
  [StringLength(20, MinimumLength = 4,
    ErrorMessageResourceName = ValidationKeys.UserNameLength, ErrorMessageResourceType = typeof(ValidationErrors))]
  public string UserName { get; init; } = null!;

  [Required(
    ErrorMessageResourceName = ValidationKeys.PasswordRequired, ErrorMessageResourceType = typeof(ValidationErrors))]
  [StringLength(20, MinimumLength = 4,
    ErrorMessageResourceName = ValidationKeys.PasswordLength, ErrorMessageResourceType = typeof(ValidationErrors))]
  [Compare("Password2",
    ErrorMessageResourceName = ValidationKeys.PasswordMismatch, ErrorMessageResourceType = typeof(ValidationErrors))]
  public string Password1 { get; init; } = null!;

  [Required(
    ErrorMessageResourceName = ValidationKeys.PasswordRequired, ErrorMessageResourceType = typeof(ValidationErrors))]
  [StringLength(20, MinimumLength = 4,
    ErrorMessageResourceName = ValidationKeys.PasswordLength, ErrorMessageResourceType = typeof(ValidationErrors))]
  [Compare("Password1",
    ErrorMessageResourceName = ValidationKeys.PasswordMismatch, ErrorMessageResourceType = typeof(ValidationErrors))]
  public string Password2 { get; init; } = null!;
}