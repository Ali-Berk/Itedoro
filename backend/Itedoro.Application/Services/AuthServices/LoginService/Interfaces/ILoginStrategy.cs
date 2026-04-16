using Itedoro.Domain.Entities.Users;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
namespace Itedoro.Application.Services.AuthServices.LoginService.Interfaces;

public interface ILoginStrategy
{
    //TODO: OAuth için handle a provider ekle isnullorempty return false.
    bool CanHandle(LoginRequest request);

    Task<User?> LoginAsync(LoginRequest request);
}