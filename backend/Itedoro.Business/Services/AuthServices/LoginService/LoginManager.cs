using Itedoro.Data;
using Itedoro.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Itedoro.Business.Shared.Result;
using Microsoft.Extensions.Configuration;
using Itedoro.Business.Services.AuthServices.TokenService;
using Itedoro.Business.Services.AuthServices.Dtos.Responses;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;

namespace Itedoro.Business.Services.AuthServices.LoginService;
public class LoginManager(
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenManager,
    ItedoroDbContext dbContext,
    IEnumerable<ILoginStrategy> strategies,
    IConfiguration config
) : ILoginService
{
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {        
        var strategy = strategies.FirstOrDefault(s => s.CanHandle(request));
        if (strategy == null) return Result<AuthResponse>.Failure("Invalid login handle.");

        var user = await strategy.LoginAsync(request);
        if (user == null) return Result<AuthResponse>.Failure("User not found.");

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Result<AuthResponse>.Failure("Wrong password.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var rehashedPassword = passwordHasher.HashPassword(user, request.Password);
            user.UpdatePasswordHash(rehashedPassword);
            await dbContext.SaveChangesAsync();
        }

        var (refreshTokenEntity, rawRefreshToken) = tokenManager.CreateRefreshToken(user.Id);
        var accessToken = tokenManager.GenerateAccessToken(user); 
        
        //Direkt alınabilir.
        var expireMinutes = config.GetValue<int>("AppSettings:ExpireMinutes");
        var expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);
        
        dbContext.RefreshTokens.Add(refreshTokenEntity);
        await dbContext.SaveChangesAsync();

        return Result<AuthResponse>.Success(new AuthResponse(
            AccessToken: accessToken,
            RefreshToken: rawRefreshToken,
            ExpiresAt: expiresAt,
            UserId: user.Id,
            UserName: user.Username));
    }
}