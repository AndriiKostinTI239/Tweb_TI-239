using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FRM.Core.DTOs/ProfileEditDto.cs
using System.ComponentModel.DataAnnotations;

namespace FRM.Core.DTOs
{
    public class ProfileEditDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }

        // Другие поля, которые можно редактировать, например, PhoneNumber
        public string PhoneNumber { get; set; }
    }
}