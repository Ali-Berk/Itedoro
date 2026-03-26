using Itedoro.Data.Entities.Roles;

namespace Itedoro.Data.Entities.Users
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        public Guid RoleId { get; private set; }
        
        public virtual Role Role { get; private set; } = null!;

        public User(
            string username,
            string email,
            string passwordHash,
            Guid id = default,
            Guid roleId = default,
            DateTime createdAt = default,
            DateTime? updatedAt = null,
            string? name = null,
            string? surname = null,
            bool isDeleted = false
        )
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            RoleId = roleId == Guid.Empty ? Guid.Parse("F9E8D7C6-B5A4-4F3E-2D1C-0B9A8F7E6D5C") : roleId;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Name = name;
            Surname = surname;
            CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
            UpdatedAt = updatedAt;
            IsDeleted = isDeleted;
        }

        public void UpdateProfile(string name, string surname)
        {
            Name = name;
            Surname = surname;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}