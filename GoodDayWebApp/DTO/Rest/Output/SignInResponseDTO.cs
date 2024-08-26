namespace GoodDayWebApp.DTO.Rest.Output
{
  public class SignInResponseDTO
  {
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Token { get; set; } = null!;
  }
}
