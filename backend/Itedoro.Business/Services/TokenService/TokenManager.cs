
using Itedoro.Data.Entities.Users;
using Microsoft.Extensions.Configuration;
﻿using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Itedoro.Business.Services.UserServices;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data;

namespace Itedoro.Business.Services.TokenService;
public class TokenManager(
    IConfiguration config,
    IUserService userManager,
    ItedoroDbContext context
) : ITokenService
{

    public string GenereteAccessToken(User user)
    {
        var refreshToken = context.RefreshTokens.FirstOrDefault(t => user.Id == t.UserId);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name.ToString()),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config.GetValue<string>("AppSettings:Token")!));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: config.GetValue<string>("AppSettings:Issuer"),
            audience: config.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GenereteRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> AsyncGenerateAndSaveRefreshToken(User user)
    {
        var refreshToken = GenereteRefreshToken();
        var refreshTokenEntity = new RefreshToken(refreshToken, user.Id, DateTime.UtcNow.AddDays(7));
        await userManager.CreateRefreshTokenAsync(refreshTokenEntity);
        return refreshToken;
    }
}