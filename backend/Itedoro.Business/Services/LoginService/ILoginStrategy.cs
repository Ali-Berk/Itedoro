using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Services.LoginService;

public interface ILoginStrategy
{
    //OAuth için handle a provider ekle isnullorempty return false.
    bool CanHandle(LoginRequestDto request);

    Task<User?> LoginAsync(LoginRequestDto request);
}