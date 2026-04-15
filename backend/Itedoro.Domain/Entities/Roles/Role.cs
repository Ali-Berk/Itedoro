using Itedoro.Domain.Entities.Users;
using Itedoro.Domain.Exceptions;

namespace Itedoro.Domain.Entities.Roles;

public class Role
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = null!;
    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    public Role(string name, Guid id = default)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = DomainException.ThrowIfNullOrWhiteSpace(name, nameof(name));
    }

    public void UpdateName(string newName)
    {
        Name = DomainException.ThrowIfNullOrWhiteSpace(newName, nameof(newName));
    }
}
