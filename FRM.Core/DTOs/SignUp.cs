using System.ComponentModel.DataAnnotations;
using FRM.Core.Enums;


namespace FRM.Core.DTOs
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен"), MinLength(8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessage = "Необходимо согласие с условиями")]
        public bool AgreeToTerms { get; set; }

        public UserRole Role { get; set; } = UserRole.Authenticated;
    }
}