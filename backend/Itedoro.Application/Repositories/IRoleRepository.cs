using Itedoro.Domain.Entities.Roles;

namespace Itedoro.Application.Repositories;

public interface IRoleRepository
{ 
    public Task<Role?> GetByNameAsync(string roleName);
}