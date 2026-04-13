using Itedoro.Data.Entities.Users;
using Itedoro.Data.Repositories.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Data.Repositories.Auth;

public class AuthRepository(ItedoroDbContext context) :IAuthRepository
{
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CheckIfUserExistsAsync(string username, string email)
    {
        return await context.Users.AnyAsync(u => u.Username == username || u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}