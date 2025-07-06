using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs.UserDtos
{
    public class UserRoleDto
    {
        [Required]
        [RegularExpression(@"^[(admin)(company)(mentor)(student)]$", ErrorMessage = "")]
        public string RoleName { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
