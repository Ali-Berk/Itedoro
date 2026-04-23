using Itedoro.Application.Repositories;
using Itedoro.Domain.Entities.Roles;
using Itedoro.Persistence.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Persistence.Repositories;

public class RoleRepository(ItedoroDbContext context) : Repository<Role>(context), IRoleRepository
{
    public Task<Role?> GetByNameAsync(string roleName)
    {
        return Context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == roleName);
    }
}