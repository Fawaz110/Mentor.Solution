using Core.Entities;
using Core.Specifications;

namespace Core.Service.Contract
{
    public interface ISocialMediaService
    {
        Task<SocialMedia> AddAsync(SocialMedia socialMedia);
        Task<List<SocialMedia>> GetAllAsync(SocialMediaSpecification specifications, bool tracking = false);
        Task<SocialMedia> GetSpcificAsync(string id, bool tracking = false);
        Task<int> UpdateAsync(string id, string? title, string? baseUrl);
        Task<int> DeleteAsync(string id);
        Task<UserSocialMedia> AddUserSocialAsync(UserSocialMedia socialMedia);
    }
}
