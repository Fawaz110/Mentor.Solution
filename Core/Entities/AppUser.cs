using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Profile { get; set; }
        public string? Cover { get; set; }
        public string Name { get; set; }
        public string? About { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public List<UserSocialMedia> SocialMediaLinks { get; set; }
    }
}
