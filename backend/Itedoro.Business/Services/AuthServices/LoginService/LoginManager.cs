using AutoMapper;
using Itedoro.Business.Services.LoginService.Dtos;
using Itedoro.Business.Services.UserServices;
using Itedoro.Business.Services.TokenService;
using Itedoro.Data.Entities.Users;
﻿using Microsoft.AspNetCore.Identity;
using Itedoro.Business.Shared.Result;
using Itedoro.Data;

namespace Itedoro.Business.Services.LoginService;
public class LoginManager(
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenManager,
    ItedoroDbContext dbContext,
    IEnumerable<ILoginStrategy> strategies
) : ILoginService
{
    public async Task<Result<AuthTokens>> LoginAsync(LoginRequestDto request)
    {        
        var strategy = strategies.FirstOrDefault(s => s.CanHandle(request));
        if (strategy == null) return Result<AuthTokens>.Failure("Invalid login handle.");

        var user = await strategy.LoginAsync(request);
        if (user == null) return Result<AuthTokens>.Failure("User not found.");

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Result<AuthTokens>.Failure("Wrong password.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var rehashedPassword = passwordHasher.HashPassword(user, request.Password);
            user.UpdatePasswordHash(rehashedPassword);
            await dbContext.SaveChangesAsync();
        }

        var (refreshTokenEntity, rawRefreshToken) = tokenManager.CreateRefreshToken(user.Id);
        var accessToken = tokenManager.GenerateAccessToken(user);

        dbContext.RefreshTokens.Add(refreshTokenEntity);
        await dbContext.SaveChangesAsync();

        return Result<AuthTokens>.Success(new AuthTokens
        {
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken
        });
    }
}