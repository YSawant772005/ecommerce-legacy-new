namespace Ecommerce.Infrastructure.Services;

/// <summary>
/// PBKDF2-SHA256 password hasher compatible with .NET 10.
/// Uses Rfc2898DeriveBytes.Pbkdf2 (static method, .NET 6+ API).
/// </summary>
public class PasswordHasher
{
    private const int Iterations = 10000;
    private const int KeyLength = 32; // 256 bits

    public string GenerateSalt()
    {
        var salt = new byte[16];
        System.Security.Cryptography.RandomNumberGenerator.Fill(salt);
        return Convert.ToBase64String(salt);
    }

    public string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var hash = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            System.Security.Cryptography.HashAlgorithmName.SHA256,
            KeyLength);
        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        var expected = HashPassword(password, salt);
        // Constant-time comparison to prevent timing attacks
        return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expected),
            Convert.FromBase64String(hash));
    }
}
