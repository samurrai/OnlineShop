using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.DataAccess;
using OnlineShop.Domain;
using OnlineShop.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services
{
    public class UserService
    {
        public UserService(OnlineShopContext context, IOptions<SecretOptions> secretOptions)
        {
            Context = context;
            JwtSecret = secretOptions.Value.JWTSecret;
        }

        public OnlineShopContext Context { get; }
        public string JwtSecret { get; }

        public async Task<string> Authenticate(string phoneNumber)
        {
            var existingUser = await Context.Users.FirstOrDefaultAsync(user => user.Phonenumber == phoneNumber);

            if (existingUser == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.MobilePhone, phoneNumber)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> Registrate(string fullName, string phoneNumber)
        {
            var existingUser = await Context.Users.FirstOrDefaultAsync(user => user.Phonenumber == phoneNumber);
            if (existingUser != null)
            {
                return null;
            }
            Context.Users.Add(new User
            {
                FullName = fullName,
                Phonenumber = phoneNumber,
                Cart = new Cart(),
                FavoriteProducts = new List<FavoriteProduct>()
            });
            await Context.SaveChangesAsync();



            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.MobilePhone, phoneNumber)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
