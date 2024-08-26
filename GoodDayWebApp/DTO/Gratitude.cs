namespace GoodDayWebApp.DTO
{
  public class Gratitude
  {
    public int Id { get; set; }
    public int User_Id { get; set; }
    public string Date { get; set; } = null!;
    public string Content { get; set; } = null!;
  }
}
