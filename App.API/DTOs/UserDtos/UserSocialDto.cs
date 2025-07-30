using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs.UserDtos
{
    public class UserSocialDto
    {
        [Required]
        public string SocialMediaId { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
