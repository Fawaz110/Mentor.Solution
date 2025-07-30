using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs.SocialMediaDtos
{
    public class SocialMediaDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string BaseUrl { get; set; }
    }
}
