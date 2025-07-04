using App.API.DTOs.UserDtos;
using App.API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Ignored;
using Core.Service.Contract;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace App.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(
            IMapper mapper,
            IAuthService authService,
            IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
            _userManager = userManager;
        }

        #region Signup endpoint
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [HttpPost("signup")] // POST: /api/v1/auth/signup
        public async Task<ActionResult<UserDto>> Signup([FromBody] SignupDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
                return BadRequest(new ApiResponse(400, "email already exists."));

            user = new AppUser
            {
                Name = model.Name,
                UserName = model.Name.Split(" ")[0] + Guid.NewGuid().ToString().Split('-')[0],
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "an error occured"));

            var mapped = _mapper.Map<UserDto>(user);

            mapped.Token = await _authService.CreateTokenAsync(user);

            return Ok(mapped);
        }
        #endregion

        #region Login endpoint
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [HttpPost("login")] // POST: /api/v1/auth/login
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return BadRequest(new ApiResponse(400, "incorrect email or password"));

            var correct = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!correct)
                return BadRequest(new ApiResponse(400, "incorrect email or password"));

            var mapped = _mapper.Map<UserDto>(user);

            mapped.Token = await _authService.CreateTokenAsync(user);

            return Ok(mapped);
        }
        #endregion

        #region Get user endpoint
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseUserDto))]
        [HttpGet("{username}")] // GET: /api/v1/auth/{username}
        public async Task<ActionResult<BaseUserDto>> GetUser([Required][EmailAddress]string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map<BaseUserDto>(user);

            return Ok(mapped);
        }
        #endregion

        #region Send confirmation email endpoint
        [HttpPost("send-email/{email}")]
        public async Task<ActionResult<ApiResponse>> SendEmail([Required][EmailAddress]string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return BadRequest(new ApiResponse(400, "invalid email"));

            double duration;

            var flag = double.TryParse(_configuration["Durations:EmailExpiration"], out duration);

            if (!flag)
                return BadRequest(new ApiResponse(400, "an error occured"));

            var sent = new Email
            {
                Code = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(duration),
                To = email
            };

            var emailSent = await _authService.SendEmailAsync(sent, MessageType.ResetPassword);

            if (!emailSent)
                return BadRequest(new ApiResponse(400, "an error occured"));

            return Ok(new ApiResponse(200, "email sent successfully"));
        }
        #endregion

        #region Reset password endpoint
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var username = User.FindFirstValue("username");

            if (username is null)
                return Unauthorized(new ApiResponse(401));

            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            if (model.Confirm != model.New)
                return BadRequest(new ApiResponse(400, "new and confirm password must be exact the same value!"));

            var email = await _authService.CheckEmailAsync(model.Code);

            if (email is null)
                return NotFound(new ApiResponse(404, "Email not found or expired, send email again."));

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.New);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion


    }
}
