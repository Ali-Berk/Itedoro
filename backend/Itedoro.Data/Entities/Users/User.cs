using Itedoro.Data.Entities.Roles;

namespace Itedoro.Data.Entities.Users
{
    public class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Username { get; set;}
        public string? Name { get; set;}
        public string? Surname { get; set;}
        public string Email { get; set;}
        public string PasswordHash { get; set;}

        public DateTime CreatedAt { get; init;} = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set;} = null;
        public bool IsDeleted { get; set;} = false;

        public Guid RoleId { get; set;} = Guid.Parse("F9E8D7C6-B5A4-4F3E-2D1C-0B9A8F7E6D5C");
        public Role Role { get; set;}
    }
}
