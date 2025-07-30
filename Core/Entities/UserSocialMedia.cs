namespace Core.Entities
{
    public class UserSocialMedia
    {
        public string AppUserId { get; set; }
        public string SocialMediaId { get; set; }
        public string Username { get; set; }
        public SocialMedia SocialMedia { get; set; }
    }
}
