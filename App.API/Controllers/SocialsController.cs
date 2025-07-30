using App.API.DTOs.SocialMediaDtos;
using App.API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Service.Contract;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class SocialsController : BaseApiController
    {
        private readonly ISocialMediaService _socialMediaService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public SocialsController(
            ISocialMediaService socialMediaService,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _socialMediaService = socialMediaService;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region Get all social media endpoint
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SocialMediaToReturnDto>))]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<SocialMediaToReturnDto>>> GetAll([FromQuery] string? titleSerachTerm = null)
        {
            var spec = new SocialMediaSpecification(titleSearchTerm: titleSerachTerm);

            var data = await _socialMediaService.GetAllAsync(spec);

            if (data.Count() == 0)
                return NotFound(new ApiResponse(404));

            var count = _userManager.Users.Count();

            var mapped = _mapper.Map<List<SocialMediaToReturnDto>>(data).Select(e => new SocialMediaToReturnDto { BaseUrl = e.BaseUrl, Id = e.Id, Title = e.Title, Users = e.Users});

            return Ok(mapped);
        }
        #endregion

        #region Add new social media as usable to integrate it to profile endpoint
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SocialMediaToReturnDto))]
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse>> Add([FromBody] SocialMediaDto model)
        {
            var item = _mapper.Map<SocialMedia>(model);

            var spec = new SocialMediaSpecification(titleSearchTerm: model.Title);

            var exist = await _socialMediaService.GetAllAsync(spec);

            if (exist.Count() != 0)
                return BadRequest(new ApiResponse(400, "same social media title exists."));

            var added = await _socialMediaService.AddAsync(item);

            if (added is null)
                return BadRequest(new ApiResponse(400));

            var mapped = _mapper.Map<SocialMediaToReturnDto>(added);

            return Ok(mapped);
        }
        #endregion

        #region Update social media endpoint
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [Authorize(Roles = "admin")]
        [HttpPatch("update")]
        public async Task<ActionResult<ApiResponse>> UpdateSocialMedia([FromBody] UpdateSocialMediaDto model)
        {
            var item = await _socialMediaService.GetSpcificAsync(model.Id);

            if (item is null)
                return NotFound(new ApiResponse(404));

            var result = await _socialMediaService.UpdateAsync(model.Title, model.Title, model.BaseUrl);

            if (result == -1)
                return NotFound(new ApiResponse(404));

            if (result == 0)
                return Ok(new ApiResponse(200, "already existed as you like"));

            return Ok(new ApiResponse(200));
        }
        #endregion

        #region Delete social media endpoint
        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteSocialMedia(string id)
        {
            var result = await _socialMediaService.DeleteAsync(id);

            return result switch
            {
                0 => Ok(new ApiResponse(200, "nothing changed")),
                -1 => BadRequest(new ApiResponse(400)),
                _ => Ok(new ApiResponse(200))
            };
        }
        #endregion

    }
}
