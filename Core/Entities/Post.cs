namespace Core.Entities
{
    public class Post : BaseEntity
    {
        public string? Caption { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<PostImage> Images { get; set; }
    }
}
