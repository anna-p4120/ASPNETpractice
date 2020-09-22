using GoodNewsApp.BusinessLogic.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoodNewsApp.BusinessLogic.Services.UsersServices
{
    public class PasswordManager
        
    {
         
        public static void CreatePasswordHash(string password, out string savedPasswordHash, out string savedPasswordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte [] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                savedPasswordHash = Convert.ToBase64String(passwordHash);
                savedPasswordSalt = Convert.ToBase64String(passwordSalt);
            }
        }

        public static bool VerifyPasswordHash(string password, string savedPasswordHash, string savedPasswordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            byte[] storedSalt = Convert.FromBase64String(savedPasswordSalt);
            byte[] storedHash = Convert.FromBase64String(savedPasswordHash);
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public static string CreateToken(string userName, Guid roleId, string appSettings)
        {

            //TODO userName replace by email

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleId.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();


            var key = Encoding.ASCII.GetBytes(appSettings);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;

            //return "1";

        }
    }

}
