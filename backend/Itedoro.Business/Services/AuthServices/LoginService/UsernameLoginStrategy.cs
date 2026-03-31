using Itedoro.Data;
using Itedoro.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Services.AuthServices.LoginService;

public class UsernameLoginStrategy(
    ItedoroDbContext dbContext
) : ILoginStrategy
{
    public bool CanHandle(LoginRequest request) => !request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequest loginRequest)
    {
        return await dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == loginRequest.LoginHandle);
    }
}