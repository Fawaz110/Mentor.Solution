using App.API.DTOs;
using App.API.DTOs.UserDtos;
using App.API.Errors;
using App.API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Service.Contract;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace App.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UsersController(
            UserManager<AppUser> userManager,
            IMapper mapper,
            IAuthService authService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
        }

        #region Profile endpoint (to be continued...) add social media
        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map<UserProfileDto>(user);

            return Ok(mapped);
        }
        #endregion

        #region Get user endpoint
        [HttpGet("{username}")]
        public async Task<ActionResult<UserProfileDto>> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map<UserProfileDto>(user);

            return Ok(mapped);
        }
        #endregion

        #region Get users endpoint
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationDto<BaseUserDto>))]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BaseUserDto>>> GetAll([FromQuery] UserPaginationQueryParams query)
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (query.Role != null)
                usersQuery = usersQuery.Where(user => user.Role == query.Role);

            if (query.Term != null)
                usersQuery = usersQuery.Where(user => user.Name.Trim().ToLower().Contains(query.Term.Trim().ToLower()));

            Expression<Func<AppUser, object>> expression;

            switch (query.OrderBy)
            {
                case "username":
                    expression = user => user.UserName;
                    break;
                case "email":
                    expression = user => user.Email;
                    break;
                default:
                    expression = user => user.Name;
                    break;
            }

            switch (query.Order)
            {
                case "asc":
                    usersQuery = usersQuery.OrderBy(expression);
                    break;
                default:
                    usersQuery = usersQuery.OrderByDescending(expression);
                    break;
            }

            // result after applying pagination.
            var users = await usersQuery.Skip(((int)query.Page - 1) * (int)query.Limit).Take((int)query.Limit).ToListAsync();
            
            if (users.Count() == 0)
                return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map<List<BaseUserDto>>(users);

            return Ok(new PaginationDto<BaseUserDto>(usersQuery.Count(), (int)query.Page, (int)query.Limit, mapped));
        }
        #endregion

        #region Update personal data endpoint
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [Authorize]
        [HttpPatch("personal-data")]
        public async Task<ActionResult<ApiResponse>> UpdatePersonalData([FromBody] PersonalDataDto model)
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            if (!string.IsNullOrEmpty(model.Name))
                user.Name = model.Name;

            if (!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Phone))
                user.PhoneNumber = model.Phone;

            if (!string.IsNullOrEmpty(model.About))
                user.About = model.About;

            if (!string.IsNullOrEmpty(model.Address))
                user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion

        #region Update profile image endpoint
        [Authorize]
        [HttpPost("profile-image")]
        public async Task<ActionResult<ApiResponse>> UpdateProfileImage([FromForm] IFormFile image)
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            if (!DocumentSettings.IsValidFile(image, DocumentSettings.ImageExtensions))
                return BadRequest(new ApiResponse(400, "invalid image file"));

            if (!string.IsNullOrEmpty(user.Profile))
                DocumentSettings.DeleteFile(user.Profile, "Images\\Profiles");

            user.Profile = DocumentSettings.UploadFile(image, "Images\\Profiles");

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion

        #region Remove profile image endpoint
        [Authorize]
        [HttpDelete("profile-image")]
        public async Task<ActionResult<ApiResponse>> DeleteProfileImage()
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            if (string.IsNullOrEmpty(user.Profile))
                return Ok(new ApiResponse(200, "profile image already removed"));

            DocumentSettings.DeleteFile(user.Profile, "Images\\Profiles");

            user.Profile = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion

        #region Update Cover image endpoint
        [Authorize]
        [HttpPost("cover-image")]
        public async Task<ActionResult<ApiResponse>> UpdateCoverImage([FromForm] IFormFile image)
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            if (!DocumentSettings.IsValidFile(image, DocumentSettings.ImageExtensions))
                return BadRequest(new ApiResponse(400, "invalid image file"));

            if (!string.IsNullOrEmpty(user.Cover))
                DocumentSettings.DeleteFile(user.Cover, "Images\\Covers");

            user.Cover = DocumentSettings.UploadFile(image, "Images\\Covers");

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion

        #region Remove Cover image endpoint
        [Authorize]
        [HttpDelete("cover-image")]
        public async Task<ActionResult<ApiResponse>> DeleteCoverImage()
        {
            var id = User.FindFirstValue("id");

            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse(404));

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResponse(404));

            if (string.IsNullOrEmpty(user.Cover))
                return Ok(new ApiResponse(200, "cover image already removed"));

            DocumentSettings.DeleteFile(user.Profile, "Images\\Covers");

            user.Cover = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }
        #endregion


    }
}
