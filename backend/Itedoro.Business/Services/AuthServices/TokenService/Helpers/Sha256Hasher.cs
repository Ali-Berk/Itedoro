using System.Security.Cryptography;
using System.Text;

namespace Itedoro.Business.Services.AuthServices.TokenService.Helpers;

internal static class Sha256Hasher
{
    public static string ComputeHash(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        byte[] inputBytes = Encoding.UTF8.GetBytes(value);
        byte[] hashBytes = SHA256.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}