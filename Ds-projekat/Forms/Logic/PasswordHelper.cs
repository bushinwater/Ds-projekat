using System;
using System.Security.Cryptography;

namespace Ds_projekat
{
    internal static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] key;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                key = pbkdf2.GetBytes(KeySize);
            }

            return Iterations + "." +
                   Convert.ToBase64String(salt) + "." +
                   Convert.ToBase64String(key);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (string.IsNullOrWhiteSpace(storedHash))
                return false;

            string[] parts = storedHash.Split('.');
            if (parts.Length != 3)
                return false;

            int iterations;
            if (!int.TryParse(parts[0], out iterations))
                return false;

            byte[] salt;
            byte[] expectedKey;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                expectedKey = Convert.FromBase64String(parts[2]);
            }
            catch
            {
                return false;
            }

            byte[] actualKey;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                actualKey = pbkdf2.GetBytes(expectedKey.Length);
            }

            return SlowEquals(actualKey, expectedKey);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null)
                return false;

            uint diff = (uint)a.Length ^ (uint)b.Length;
            int length = Math.Min(a.Length, b.Length);

            for (int i = 0; i < length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}