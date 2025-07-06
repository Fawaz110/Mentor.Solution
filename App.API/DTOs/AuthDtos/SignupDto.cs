using System.ComponentModel.DataAnnotations;

namespace App.API.DTOs.UserDtos
{
    public class SignupDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+2)?01[0125][0-9]{8}$", ErrorMessage = "Invalid egyption phone number")]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "At least 8 characters\r\n\r\nAt least one lowercase letter ([a-z])\r\n\r\nAt least one uppercase letter ([A-Z])\r\n\r\nAt least one digit (\\d)\r\n\r\nAt least one special character from @$!%*?&\r\n\r\nOnly allows characters from: A-Z, a-z, 0-9, and @$!%*?&")]
        public string Password { get; set; }

    }
}
