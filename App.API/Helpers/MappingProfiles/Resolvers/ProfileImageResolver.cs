using App.API.DTOs.UserDtos;
using AutoMapper;
using Core.Entities;

namespace App.API.Helpers.MappingProfiles.Resolvers
{
    public class ProfileImageResolver : IValueResolver<AppUser, BaseUserDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProfileImageResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, BaseUserDto destination, string destMember, ResolutionContext context)
            => (string.IsNullOrEmpty(source.Profile)) ? null : Path.Combine(_configuration["URLS:https"], "Images", "Profiles", source.Profile);
        
    }
}
