using AutoMapper;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.UserServices;
using Itedoro.Business.Services.TokenService;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Services.LoginService;
public class LoginManager : ILoginService
{
    ITokenService _tokenManager;
    private readonly IUserService _userManager;
    private readonly IEnumerable<ILoginStrategy> _strategies;
    private readonly IMapper _mapper;
    public LoginManager(IUserService userManager, IEnumerable<ILoginStrategy> strategies, IMapper mapper, ITokenService tokenManager)
    {
        _tokenManager = tokenManager;
        _userManager = userManager;
        _strategies = strategies;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {        
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request));
        if (strategy == null) return null;

        var user = await strategy.LoginAsync(request);
        if (user == null) return null;

        if (!_userManager.VerifyPassword(user, request.Password)) return null;
        
        var response = _mapper.Map<LoginResponseDto>(user);

        response.RefreshToken = await _tokenManager.AsyncGenerateAndSaveRefreshToken(user);
        response.AccessToken = _tokenManager.GenereteAccessToken(user);
        response.ExpiresAt = DateTime.UtcNow.AddDays(1);
        
        return response;
    }
}