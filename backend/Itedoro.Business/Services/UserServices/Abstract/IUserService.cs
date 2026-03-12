using Microsoft.AspNetCore.Identity;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Services.UserServices
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(User user, string password);

        Task<User?> GetByEmailAsync(string user);
        Task<User?> GetByUsernameAsync(string user);

        bool VerifyPassword(User user, string password);

    }
}