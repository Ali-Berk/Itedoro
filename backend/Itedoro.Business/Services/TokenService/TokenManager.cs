namespace Itedoro.Business.Services.TokenService;

using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Services.TokenService.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Itedoro.Data;
using Itedoro.Data.Entities.Users;
using Itedoro.Business.Shared.Result;

public class TokenManager(
    IConfiguration config,
    ItedoroDbContext context
) : ITokenService
{
    // getvalue'lar constructor içinde tanımlanacak.
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
        };

        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config.GetValue<string>("AppSettings:Token")!));
        
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);
        var expiresMinutes = config.GetValue<int>("AppSettings:ExpireMinutes");

        var jwtToken = new JwtSecurityToken(
            issuer: config.GetValue<string>("AppSettings:Issuer"),
            audience: config.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public (RefreshToken Entity, string rawToken) CreateRefreshToken(Guid userId)
    {
        var expireDays = config.GetValue<int>("AppSettings:ExpireDays");
        
        string rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        
        return
        (
            new RefreshToken(
                Sha256Hasher.ComputeHash(rawToken),
                userId,
                DateTime.UtcNow.AddDays(expireDays)),
                rawToken);
    }

    public async Task<Result<string>> RefreshAsync(string refreshToken)
    {
        var hashedToken = Sha256Hasher.ComputeHash(refreshToken);
        var exist = await context.RefreshTokens
            .Include(t => t.User).Include(t => t.User.Role)
            .FirstOrDefaultAsync(t => t.Token == hashedToken);

        if (exist == null || exist.IsExpired)
        {
            return Result<string>.Failure(errors: new[] { "Invalid Refresh Token." });
        }
        
        var accessToken = GenerateAccessToken(exist.User);
        return Result<string>.Success(accessToken);
    }
}