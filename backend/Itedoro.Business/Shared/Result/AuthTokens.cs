namespace Itedoro.Business.Shared.Result;

public class AuthTokens
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}