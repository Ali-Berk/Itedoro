using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Data.Entities.Users;
using Itedoro.Data;
using Microsoft.EntityFrameworkCore;
namespace Itedoro.Business.Services.LoginService;

public class EmailLoginStrategy(
    ItedoroDbContext dbContext
) : ILoginStrategy
{
    public bool CanHandle(LoginRequestDto request) => request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequestDto loginRequest)
    {
        return await dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginRequest.LoginHandle);
    }
}