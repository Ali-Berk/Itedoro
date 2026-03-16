using System.ComponentModel.DataAnnotations;

namespace Itedoro.Data.Entities.Users;

public class RefreshToken
{
    [Key]
    public Guid TokenId { get; init;}
    public string Token { get; set;} = null!;
    public DateTime ExpiryTime { get; set;}
    public DateTime CreatedAt { get; init;} = DateTime.UtcNow;
    public Guid UserId { get; init;}
    public User user { get; init;} = null!;
    public bool IsExpired => ExpiryTime < DateTime.UtcNow;

    private RefreshToken(){}
    public RefreshToken(string token, Guid userId, DateTime expiryTime, DateTime createdAt = default)
    {
        TokenId = Guid.NewGuid();
        Token = token;
        ExpiryTime = expiryTime;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UserId = userId;
    }
}