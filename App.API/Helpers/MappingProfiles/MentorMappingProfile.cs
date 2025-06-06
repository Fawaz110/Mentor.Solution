using App.API.DTOs.UserDtos;
using App.API.Helpers.MappingProfiles.Resolvers;
using AutoMapper;
using Core.Entities;

namespace App.API.Helpers.MappingProfiles
{
    public class MentorMappingProfile : Profile
    {
        public MentorMappingProfile()
        {
            CreateMap<AppUser, BaseUserDto>()
                .ForMember(dist => dist.Profile, O => O.MapFrom<ProfileImageResolver>());

            CreateMap<AppUser, UserDto>()
                .ForMember(dist => dist.Profile, O => O.MapFrom<ProfileImageResolver>());
        }
    }
}
