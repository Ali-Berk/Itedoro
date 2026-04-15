using Itedoro.Domain.Entities.Users;
using Itedoro.Domain.Exceptions;

namespace Itedoro.Domain.Entities.RefreshTokens;

public class RefreshToken
{
    public Guid TokenId { get; init; }
    public string Token { get; private set; }
    public DateTime ExpiryTime { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public bool IsExpired => ExpiryTime <= DateTime.UtcNow;

    public RefreshToken(string token, Guid userId, DateTime expiryTime, Guid tokenId = default, DateTime createdAt = default)
    {
        TokenId = tokenId == Guid.Empty ? Guid.NewGuid() : tokenId;
        Token = DomainException.ThrowIfNullOrWhiteSpace(token, nameof(token));
        UserId = DomainException.ThrowIfEmpty(userId, nameof(userId));
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        DomainException.ThrowIf(expiryTime <= CreatedAt, "expiryTime must be later than the creation time.");
        ExpiryTime = expiryTime;
    }

    public void Revoke()
    {
        if (IsExpired)
        {
            return;
        }

        ExpiryTime = DateTime.UtcNow;
    }
}
