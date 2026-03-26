using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Entities.Roles
{
    public class Role
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public Role(
            string name,
            Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name;
        }
        public void UpdateName(string newName)
        {
            Name = newName;
        }
    }
}