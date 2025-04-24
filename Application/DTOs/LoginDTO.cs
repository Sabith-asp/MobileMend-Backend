using System.ComponentModel.DataAnnotations;

namespace MobileMend.Application.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers (no spaces or special characters).")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).{6,}$",
            ErrorMessage = "Password must contain at least one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
