using Itedoro.Data.Entities.Users;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;
namespace Itedoro.Business.Services.AuthServices.LoginService;

public interface ILoginStrategy
{
    //TODO: OAuth için handle a provider ekle isnullorempty return false.
    bool CanHandle(LoginRequest request);

    Task<User?> LoginAsync(LoginRequest request);
}