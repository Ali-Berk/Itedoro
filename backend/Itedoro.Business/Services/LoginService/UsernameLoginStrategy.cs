using Itedoro.Business.Services.LoginService.Dtos;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data.Entities.Users;
using Itedoro.Data;

namespace Itedoro.Business.Services.LoginService;

public class UsernameLoginStrategy(
    ItedoroDbContext dbContext
) : ILoginStrategy
{
    public bool CanHandle(LoginRequestDto request) => !request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequestDto loginRequest)
    {
        return await dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == loginRequest.LoginHandle);
    }
}