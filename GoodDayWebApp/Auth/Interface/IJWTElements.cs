namespace GoodDayWebApp.Auth.Interface
{
  public interface IJWTElements
  {
    string Audience { get; init; }
    string Issuer { get; init; }
    string Key { get; init; }

    void Deconstruct(out string Key, out string Issuer, out string Audience);
    bool Equals(JWTElements? other);
    bool Equals(object? obj);
    int GetHashCode();
    string ToString();
  }
}