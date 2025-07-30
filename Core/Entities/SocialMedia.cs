namespace Core.Entities
{
    public class SocialMedia : BaseEntity
    {
        public string Title { get; set; }
        public string BaseUrl { get; set; }
        // icon of social better to be here depends on package used in frontend (fontawesome, primeng, ...)
        public List<UserSocialMedia> SocialMediaLinks { get; set; }
    }
}
