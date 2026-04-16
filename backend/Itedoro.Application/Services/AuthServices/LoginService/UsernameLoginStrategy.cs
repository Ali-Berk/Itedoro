using Itedoro.Domain.Entities.Users;
using Itedoro.Application.Repositories;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Services.AuthServices.LoginService.Interfaces;

namespace Itedoro.Application.Services.AuthServices.LoginService;

public class UsernameLoginStrategy(
    IAuthRepository repository
    ) : ILoginStrategy
{
    public bool CanHandle(LoginRequest request) => !request.LoginHandle.Contains("@");
    public async Task<User?> LoginAsync(LoginRequest loginRequest)
    {
        return await repository.GetUserByUsernameAsync(loginRequest.LoginHandle);
    }
}