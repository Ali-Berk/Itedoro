using System.ComponentModel.DataAnnotations;
namespace Itedoro.Data.Entities.Users
{
    public class RefreshToken
    {
        [Key]
        public Guid TokenId { get; init; }
        
        public string Token { get; private set; } = null!;
        public DateTime ExpiryTime { get; private set; }
        public DateTime CreatedAt { get; init; }
        
        public Guid UserId { get; private set; }
        
        public User User { get; private set; } = null!;

        public bool IsExpired => ExpiryTime < DateTime.UtcNow;

        protected RefreshToken() { }

        public RefreshToken(string token, Guid userId, DateTime expiryTime)
        {
            TokenId = Guid.NewGuid();
            Token = token;
            UserId = userId;
            ExpiryTime = expiryTime;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            ExpiryTime = DateTime.UtcNow.AddSeconds(-1);
        }
    }
}