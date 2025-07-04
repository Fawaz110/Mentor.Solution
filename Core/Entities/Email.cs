namespace Core.Entities
{
    public class Email : BaseEntity
    {
        public string Code { get; set; }
        public string To { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
