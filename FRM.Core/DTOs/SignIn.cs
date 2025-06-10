using System.ComponentModel.DataAnnotations;

namespace FRM.Core.DTOs
{
    public class SignInDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}