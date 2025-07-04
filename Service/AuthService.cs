using Core.Entities;
using Core.Entities.Ignored;
using Core.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Core;
using Core.Specifications;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
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

        /// <summary>
        /// Check if email exists and valid or not.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>returns null if email expired or doesn't exist. Otherwise returns email</returns>
        public async Task<Email> CheckEmailAsync(string code)
        {
            var spec = new BaseSpecification<Email>(e => e.Code == code);

            var email = await _unitOfWork.Repository<Email>().GetWithSpecAsync(spec);

            if (email is null)
                return null;

            if(DateTime.UtcNow > email?.ExpiresAt)
            {
                _unitOfWork.Repository<Email>().Delete(email);

                await _unitOfWork.CompleteAsync();

                return null;
            }

            return email;
        }

        public async Task<bool> SendEmailAsync(Email email, MessageType type = MessageType.ResetPassword)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("mmohamedfawzi23@gmail.com", "zpzguhnqzaufmwmm");

                var route = (type == MessageType.VarifyEmail) ? "/auth/confirm-email/" : "/auth/reset-password/";

                var btn = (type == MessageType.VarifyEmail) ? "Varify Email" : "Reset Password";

                var confirmationLink = _configuration["URLS:AngularApp"] + route + email.Code;

                int duration;

                var flag = int.TryParse(_configuration["Durations:EmailExpiration"], out duration);

                var body = $@"
                                <!DOCTYPE html>
                                <html>
                                <head>
                                <style>
                                .button {{
                                  background-color: #4CAF50; /* Green */
                                  border: none;
                                  color: white;
                                  padding: 15px 32px;
                                  text-align: center;
                                  text-decoration: none;
                                  display: inline-block;
                                  font-size: 16px;
                                  margin: 4px 2px;
                                  cursor: pointer;
                                  border-radius: 5px; /* Rounded corners */
                                }}
                                </style>
                                </head>
                                <body>

                                <p>Thanks for keeping in touch with Musica! We're excited to have you on board and will be happy to help you set everything up.</p>
                                <p>Click below to varify your email: <a href='{email.To}'>{email.To}</a></p><br><br>
                                <a href='{confirmationLink}' target='_blank'><button class='button'>{btn}</button></a>
                                <p>Varification link expires after {duration} minutes</p>
                                </body>
                                </html>";

                var message = new MailMessage("mmohamedfawzi23@gmail.com", email.To, btn, body);

                message.IsBodyHtml = true;

                await client.SendMailAsync(message);

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
