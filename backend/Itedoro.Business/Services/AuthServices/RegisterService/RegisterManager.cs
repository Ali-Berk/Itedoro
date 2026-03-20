using Microsoft.AspNetCore.Identity;
using Itedoro.Data.Entities.Users;
using Itedoro.Data;
using Itedoro.Business.Services.RegisterService.Dtos;
using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.TokenService;

namespace Itedoro.Business.Services.RegisterService;
public class RegisterManager(
    ItedoroDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService
) : IRegisterService
{

    public async Task<Result<AuthTokens>> RegisterAsync(RegisterRequestDto request)
    {        
        var isUserExist = await dbContext.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username);
        if (isUserExist)
        {
            return Result<AuthTokens>.Failure("User already exists.");
        }
        
        var user = new User(request.Username, request.Email, "placeholder");
        
        string hashedPassword = passwordHasher.HashPassword(user, request.Password);
        user.UpdatePasswordHash(hashedPassword);

        dbContext.Users.Add(user);

        var (refreshToken, rawRefreshToken) = tokenService.CreateRefreshToken(user.Id);
        var accessToken = tokenService.GenerateAccessToken(user);

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync();

        return Result<AuthTokens>.Success(new AuthTokens
        {
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken
        });
    }
}