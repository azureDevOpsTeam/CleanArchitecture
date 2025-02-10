#region Usings

using System.Security.Cryptography;
using System.Text;

#endregion

namespace ApplicationLayer.Extensions
{
    public static class HashGenerator
    {
        public static string SecurityPepper = "C4qWkpZ8CEs3A9Ks";

        public static string GenerateSHA256HashWithSalt(string password, out string securityStamp)
        {
            byte[] saltBytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new())
            {
                rng.GetBytes(saltBytes);
            }
            securityStamp = Convert.ToBase64String(saltBytes);
            byte[] salt = Encoding.UTF8.GetBytes(securityStamp + SecurityPepper);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateHashChangePassword(string password, string securityStamp)
        {
            byte[] salt = Encoding.UTF8.GetBytes(securityStamp + SecurityPepper);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword, string securityStamp)
        {
            securityStamp += SecurityPepper;
            byte[] saltBytes = Encoding.UTF8.GetBytes(securityStamp);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == hashedPassword;
            }
        }
    }
}