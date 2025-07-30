using Core;
using Core.Entities;
using Core.Service.Contract;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public SocialMediaService(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        
        public async Task<SocialMedia> AddAsync(SocialMedia socialMedia)
        {
            _unitOfWork.Repository<SocialMedia>().AddAsync(socialMedia);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return socialMedia;
        }

        public async Task<UserSocialMedia> AddUserSocialAsync(UserSocialMedia socialMedia)
        {
            var item = await _unitOfWork.Repository<SocialMedia>().GetByIdWithNoTrackingAsync(socialMedia.SocialMediaId);

            if (item is null)
                return null;

            var user = await _userManager.Users
                                .Include(u => u.SocialMediaLinks)
                                .FirstOrDefaultAsync(u => u.Id == socialMedia.AppUserId);

            if (user is null)
                return null;

            var existed = user.SocialMediaLinks.Any(s => s.SocialMediaId == socialMedia.SocialMediaId);

            // Check if social not existed before add it as new one. if existed modify it's username.
            if (!existed)
                user.SocialMediaLinks.Add(socialMedia);
            else
                foreach (var link in user.SocialMediaLinks)
                    if (link.SocialMediaId == socialMedia.SocialMediaId)
                    {
                        link.Username = socialMedia.Username;
                        break;
                    }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return socialMedia;

            return null;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var spec = new SocialMediaSpecification(id);

            var item = await _unitOfWork.Repository<SocialMedia>().GetWithSpecAsync(spec);

            if (item is null)
                return 0;

            _unitOfWork.Repository<SocialMedia>().Delete(item);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return -1;

            return result;
        }

        public async Task<List<SocialMedia>> GetAllAsync(SocialMediaSpecification specifications, bool tracking = false)
        {
            var data = tracking ? await _unitOfWork.Repository<SocialMedia>().GetAllAsync(specifications)
                       : await _unitOfWork.Repository<SocialMedia>().GetAllWithNoTrackingAsync(specifications);
            
            return data.ToList();
        }

        public async Task<SocialMedia> GetSpcificAsync(string id, bool tracking = false)
        {
            var spec = new SocialMediaSpecification(id);

            return tracking ? await _unitOfWork.Repository<SocialMedia>().GetByIdAsync(id) 
                            : await _unitOfWork.Repository<SocialMedia>().GetByIdWithNoTrackingAsync(id);
        }

        public async Task<int> UpdateAsync(string id, string? title, string? baseUrl)
        {
            var spec = new SocialMediaSpecification(id);

            var item = await _unitOfWork.Repository<SocialMedia>().GetWithSpecAsync(spec);

            if (item is null)
                return -1;

            if (!string.IsNullOrEmpty(title))
                item.Title = title;

            if (!string.IsNullOrEmpty(baseUrl))
                item.BaseUrl = baseUrl;

            _unitOfWork.Repository<SocialMedia>().Update(item);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return 0;

            return result;
        }
    }
}
