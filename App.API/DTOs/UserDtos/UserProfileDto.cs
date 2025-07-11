namespace App.API.DTOs.UserDtos
{
    public class UserProfileDto : BaseUserDto
    {
        public string? About { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
