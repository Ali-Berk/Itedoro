using AutoMapper;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.UserServices;

namespace Itedoro.Business.Services.LoginService;
public class LoginManager : ILoginService
{
    private readonly IUserService _userManager;
    private readonly IEnumerable<ILoginStrategy> _strategies;
    private readonly IMapper _mapper;
    public LoginManager(IUserService userManager, IEnumerable<ILoginStrategy> strategies, IMapper mapper)
    {
        _userManager = userManager;
        _strategies = strategies;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {        
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request)) 
            ?? throw new NotSupportedException("Invalid Login type or missing requirements.");

        var result = await strategy.LoginAsync(request);
        if (result == null) return null;
        if (!_userManager.VerifyPassword(result, request.Password)) return null;
        return _mapper.Map<LoginResponseDto>(result);
    }
}