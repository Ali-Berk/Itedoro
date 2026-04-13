using Microsoft.AspNetCore.Identity;
using Itedoro.Data.Entities.Users;
using Itedoro.Business.Shared.Result;
using Itedoro.Business.Services.AuthServices.TokenService;
using Itedoro.Business.Services.AuthServices.Dtos.Requests;
using Itedoro.Data.Repositories.Auth.Interfaces;
using Itedoro.Data.Repositories.RefreshToken.Interfaces;

namespace Itedoro.Business.Services.AuthServices.RegisterService;
public class RegisterManager(
    IAuthRepository repository,
    IRefreshTokenRepository tokenRepository,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService
) : IRegisterService
{

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        
        if (await repository.CheckIfUserExistsAsync(request.Username, request.Email))
        {
            return Result.Failure("User already exists.");
        }
        
        var user = new User(request.Username, request.Email, "placeholder");
        
        string hashedPassword = passwordHasher.HashPassword(user, request.Password);
        user.UpdatePasswordHash(hashedPassword);

        await repository.AddUserAsync(user);

        var (refreshToken, rawRefreshToken) = tokenService.CreateRefreshToken(user.Id);
        var accessToken = tokenService.GenerateAccessToken(user);
        
        //Raw ve access fronta yollanacaktı. değiştirilecek.
        await tokenRepository.AddAsync(refreshToken);
        await repository.SaveChangesAsync();
        return Result.Success();
    }
}