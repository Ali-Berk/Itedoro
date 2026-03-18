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
        
        public Role Role { get; private set; } = null!;

        protected User() { }
        public User(string username, string email, string passwordHash, Guid? roleId = null)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            
            RoleId = roleId ?? Guid.Parse("F9E8D7C6-B5A4-4F3E-2D1C-0B9A8F7E6D5C");
            
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
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