using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs.SocialMediaDtos
{
    public class UpdateSocialMediaDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string BaseUrl { get; set; }
    }
}
