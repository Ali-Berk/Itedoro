
using Itedoro.Business.Services.LoginService.Dtos;

namespace Itedoro.Business.Services.LoginService
{
    public interface ILoginService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
    
}