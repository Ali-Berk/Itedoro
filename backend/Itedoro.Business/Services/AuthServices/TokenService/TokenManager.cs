using System.Text;
using Itedoro.Data;
using System.Security.Claims;
using Itedoro.Data.Entities.Users;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Shared.Result;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Itedoro.Business.Services.TokenService.Helpers;
namespace Itedoro.Business.Services.AuthServices.TokenService;

public class TokenManager : ITokenService
{
    private readonly ItedoroDbContext context;
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpiresMinutes;
    private readonly int _refreshTokenExpiresDays;

    public TokenManager(ItedoroDbContext _context, IConfiguration _config)
    {
        context = _context;
        config = _config;
        
        var tokenKey = config.GetValue<string>("AppSettings:Token") 
                       ?? throw new Exception("Token key not found in settings");
            
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        _issuer = config.GetValue<string>("AppSettings:Issuer") ?? "";
        _audience = config.GetValue<string>("AppSettings:Audience") ?? "";
        _accessTokenExpiresMinutes = config.GetValue<int>("AppSettings:ExpireMinutes");
        _refreshTokenExpiresDays = config.GetValue<int>("AppSettings:ExpireDays");
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiresMinutes),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public (RefreshToken Entity, string rawToken) CreateRefreshToken(Guid userId)
    {
        string rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        
        return
        (
            new RefreshToken(
                Sha256Hasher.ComputeHash(rawToken),
                userId,
                DateTime.UtcNow.AddDays(_refreshTokenExpiresDays)),
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
            return Result<string>.Failure("Invalid Refresh Token.");
        }
        exist.Revoke();
        
        var accessToken = GenerateAccessToken(exist.User);
        await context.SaveChangesAsync();
        return Result<string>.Success(accessToken);
    }
}