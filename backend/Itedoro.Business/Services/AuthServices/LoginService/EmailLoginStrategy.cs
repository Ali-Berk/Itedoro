using Itedoro.Data.Entities.Users;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;
using Itedoro.Data.Repositories.Auth.Interfaces;

namespace Itedoro.Business.Services.AuthServices.LoginService;

public class EmailLoginStrategy(
    IAuthRepository repository
) : ILoginStrategy
{
    public bool CanHandle(LoginRequest request) => request.LoginHandle.Contains('@');
    public async Task<User?> LoginAsync(LoginRequest loginRequest)
    {
        return await repository.GetUserByEmailAsync(loginRequest.LoginHandle);
    }
}