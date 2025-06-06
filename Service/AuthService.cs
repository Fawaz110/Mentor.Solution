using Core.Entities;
using Core.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("username", user.UserName),
                new Claim("email", user.Email),
                new Claim("phone", user.PhoneNumber),
            };

            if (!string.IsNullOrEmpty(user.Profile))
                authClaims.Add(new Claim("profile", Path.Combine(_configuration["URLS:Default"], "Images", "Profiles", user.Profile)));

            if (!string.IsNullOrEmpty(user.Cover))
                authClaims.Add(new Claim("cover", Path.Combine(_configuration["URLS:Default"], "Images", "Covers", user.Cover)));

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaims.Add(new Claim("roles", role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
