using Itedoro.Domain.Entities.Users;

namespace Itedoro.Application.Repositories;
public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username);

    Task<User?> GetUserByEmailAsync(string email);

    Task<bool> CheckIfUserExistsAsync(string username, string email);

    Task AddUserAsync(User user);

    Task<int> SaveChangesAsync();
}