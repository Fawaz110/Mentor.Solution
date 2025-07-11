using App.API.DTOs;
using AutoMapper;
using Core.Entities;

namespace App.API.Helpers.MappingProfiles.Resolvers
{
    public class CoverImageResolver : IValueResolver<AppUser, BaseUserDto, string>
    {
        private readonly IConfiguration _configuration;

        public CoverImageResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, BaseUserDto destination, string destMember, ResolutionContext context)
            => (string.IsNullOrEmpty(source.Cover)) ? null : Path.Combine(_configuration["URLS:https"], "Images", "Covers", source.Cover);

    }
}
