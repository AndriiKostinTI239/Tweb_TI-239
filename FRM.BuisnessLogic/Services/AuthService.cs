using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using FRM.BuisnessLogic.Helper;
using FRM.Core.DTOs;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using FRM.Domain.Repositories;
using FRM.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace FRM.BuisnessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly Hasher _hasher;

        public AuthService(IUserRepository userRepo, Hasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        public async Task<bool> SignUpAsync(SignUpDto dto)
        {
            if (dto.Password != dto.RepeatPassword)
                return false;

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                return false;

            var user = new UserEf
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                HashPassword = _hasher.HashPassword(dto.Password),
                AgreeToTerms = dto.AgreeToTerms,
                Role = dto.Role
            };

            await _userRepo.CreateAsync(user);

            return await SignInAsync(new SignInDto { Email = dto.Email, Password = dto.Password });
        }


        public async Task<bool> SignInAsync(SignInDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null || user.IsBanned || !_hasher.VerifyPassword(dto.Password, user.HashPassword))
                return false;

            var userData = $"{user.Id:N}|{user.Role}";

            var ticket = new FormsAuthenticationTicket(
                version: 2,
                name: user.Email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddHours(1),
                isPersistent: false,
                userData: userData);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);

            return true;
        }

        public string GenerateJwtToken(UserEf user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}