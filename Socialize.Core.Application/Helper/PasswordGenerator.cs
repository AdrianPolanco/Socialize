using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Socialize.Core.Application.Helper
{
    public static class PasswordGenerator
    {
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 12,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonAlphanumeric = true
            };

            string[] randomChars = new[]
            {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // Uppercase
            "abcdefghijkmnopqrstuvwxyz",    // Lowercase
            "0123456789",                   // Digits
            "!@$?_-"                        // Non-alphanumeric
            };

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            var chars = new char[opts.RequiredLength];
            var randomBytes = new byte[opts.RequiredLength * 2];

            rng.GetBytes(randomBytes);

            for (int i = 0; i < opts.RequiredLength; i++)
            {
                chars[i] = randomChars[randomBytes[i] % randomChars.Length][randomBytes[i + opts.RequiredLength] % randomChars[randomBytes[i] % randomChars.Length].Length];
            }

            if (opts.RequireDigit && !chars.Any(c => char.IsDigit(c)))
            {
                chars[randomBytes.Last() % opts.RequiredLength] = randomChars[2][randomBytes.First() % randomChars[2].Length];
            }

            if (opts.RequireLowercase && !chars.Any(c => char.IsLower(c)))
            {
                chars[randomBytes.Last() % opts.RequiredLength] = randomChars[1][randomBytes.First() % randomChars[1].Length];
            }

            if (opts.RequireUppercase && !chars.Any(c => char.IsUpper(c)))
            {
                chars[randomBytes.Last() % opts.RequiredLength] = randomChars[0][randomBytes.First() % randomChars[0].Length];
            }

            if (opts.RequireNonAlphanumeric && !chars.Any(c => !char.IsLetterOrDigit(c)))
            {
                chars[randomBytes.Last() % opts.RequiredLength] = randomChars[3][randomBytes.First() % randomChars[3].Length];
            }

            return new string(chars);
        }
    }
}
