// FRM.Core.DTOs/ChangePasswordDto.cs
using System.ComponentModel.DataAnnotations;

namespace FRM.Core.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Текущий пароль обязателен")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не менее 8 символов")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }
    }
}