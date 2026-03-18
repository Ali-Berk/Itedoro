using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Entities.Roles
{
    public class Role
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        protected Role() { }
        public void UpdateName(string newName)
        {
            Name = newName;
        }
    }
}