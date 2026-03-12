using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Entities.Roles
{
    public class Role
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set;}
        public ICollection<User> Users { get; set;}
    }
}