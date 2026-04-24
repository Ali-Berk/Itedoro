using Itedoro.Persistence.Repositories.Repository;
using Itedoro.Application.Repositories;
using Itedoro.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Persistence.Repositories.UserRepositories;

public class UserRepository(ItedoroDbContext context) : Repository<Itedoro.Domain.Entities.Users.User>(context), IUserRepository
{
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await Context.Users.FirstOrDefaultAsync(x => x.Username == userName, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}