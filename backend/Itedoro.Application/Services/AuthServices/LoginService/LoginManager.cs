using Itedoro.Application.Common.Errors;
using Itedoro.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Itedoro.Application.Common.Shared.Results;
using Microsoft.Extensions.Configuration;
using Itedoro.Application.Services.AuthServices.LoginService.Interfaces;
using Itedoro.Application.Services.AuthServices.TokenService.Interfaces;
using Itedoro.Application.Services.AuthServices.Dtos.Responses;
using Itedoro.Application.Services.AuthServices.Dtos.Requests;
using Itedoro.Application.Repositories;
using Itedoro.Application.Services.AuthServices.Errors;

namespace Itedoro.Application.Services.AuthServices.LoginService;
public class LoginManager(
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenManager,
    IAuthRepository repository,
    IRefreshTokenRepository refreshTokenRepository,
    IEnumerable<ILoginStrategy> strategies,
    IConfiguration config
) : ILoginService
{
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {        
        var strategy = strategies.FirstOrDefault(s => s.CanHandle(request));
        if (strategy == null) return AuthErrors.InvalidLoginHandle;

        var user = await strategy.LoginAsync(request);
        if (user == null) return CommonErrors.NotFound;

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed) return AuthErrors.InvalidCredentials;
        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var rehashedPassword = passwordHasher.HashPassword(user, request.Password);
            user.UpdatePasswordHash(rehashedPassword);
            await repository.SaveChangesAsync();
        }

        var (refreshTokenEntity, rawRefreshToken) = tokenManager.CreateRefreshToken(user.Id);
        var accessToken = tokenManager.GenerateAccessToken(user); 
        
        var expireMinutes = config.GetValue<int>("JwtSettings:AccessTokenExpireMinutes");
        var expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);
        await refreshTokenRepository.AddAsync(refreshTokenEntity);
        await refreshTokenRepository.SaveAsync();

        return Result<AuthResponse>.Success(new AuthResponse(
            AccessToken: accessToken,
            RefreshToken: rawRefreshToken,
            ExpiresAt: expiresAt,
            UserId: user.Id,
            UserName: user.Username));
    }
}
