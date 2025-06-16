// FRM.Core/DTOs/UserProfileViewDto.cs
using System;
using FRM.Core.Enums;

namespace FRM.Core.DTOs
{
    public class UserProfileViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } // Добавим для полноты
        public UserRole Role { get; set; }
        public bool IsBanned { get; set; }
        public bool AgreeToTerms { get; set; }

        // Сюда можно добавить и другую информацию, например, дату регистрации
    }
}