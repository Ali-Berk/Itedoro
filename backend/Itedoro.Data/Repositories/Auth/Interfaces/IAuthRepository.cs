using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Repositories.Auth.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> CheckIfUserExistsAsync(string username, string email);
    Task AddUserAsync(User user);
    Task<int> SaveChangesAsync();
}