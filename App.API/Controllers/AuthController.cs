using App.API.DTOs.UserDtos;
using App.API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace App.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(
            IMapper mapper,
            IAuthService authService,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _authService = authService;
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
        [HttpGet("{email}")] // GET: /api/v1/auth/{email}
        public async Task<ActionResult<BaseUserDto>> GetUser([Required][EmailAddress]string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map<BaseUserDto>(user);

            return Ok(mapped);
        }
        #endregion


    }
}
