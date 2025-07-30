using App.API.DTOs;
using App.API.DTOs.SocialMediaDtos;
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
                .ForMember(dist => dist.Profile, O => O.MapFrom<ProfileImageResolver>())
                .ForMember(dist => dist.Cover, O => O.MapFrom<CoverImageResolver>())
                .ForMember(dist => dist.SocialMedia,
                           O => O.MapFrom(src => src.SocialMediaLinks
                                .Select(link => new UserSocialMediaDto
                                {
                                    Link = link.SocialMedia.BaseUrl + link.Username,
                                    Title = link.SocialMedia.Title
                                })
                           )
                          );

            CreateMap<AppUser, UserDto>()
                .ForMember(dist => dist.Profile, O => O.MapFrom<ProfileImageResolver>())
                .ForMember(dist => dist.Cover, O => O.MapFrom<CoverImageResolver>())
                .ForMember(dist => dist.SocialMedia,
                           O => O.MapFrom(src => src.SocialMediaLinks
                                .Select(link => new UserSocialMediaDto
                                {
                                    Link = link.SocialMedia.BaseUrl + link.Username,
                                    Title = link.SocialMedia.Title
                                })
                           )
                          );

            CreateMap<AppUser, UserProfileDto>()
                .ForMember(dist => dist.Profile, O => O.MapFrom<ProfileImageResolver>())
                .ForMember(dist => dist.Cover, O => O.MapFrom<CoverImageResolver>())
                .ForMember(dist => dist.SocialMedia,
                           O => O.MapFrom(src => src.SocialMediaLinks
                                .Select(link => new UserSocialMediaDto
                                {
                                    Link = link.SocialMedia.BaseUrl + link.Username,
                                    Title = link.SocialMedia.Title
                                })
                           )
                          );

            CreateMap<SocialMedia, SocialMediaToReturnDto>()
                .ForMember(dist => dist.Users, O => O.MapFrom(src => src.SocialMediaLinks.Count()));

            CreateMap<SocialMediaDto, SocialMedia>();
        }
    }
}
