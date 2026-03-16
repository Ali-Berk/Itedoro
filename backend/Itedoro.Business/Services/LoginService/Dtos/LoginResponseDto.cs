namespace Itedoro.Business.Services.LoginService;
public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }

    public UserProfileDto User { get; set; } = new();

    public bool IsSuccess { get; set; } = true;
    public string? Message { get; set; }
}

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}