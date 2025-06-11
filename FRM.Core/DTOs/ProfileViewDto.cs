// FRM.Core.DTOs/ProfileViewDto.cs
using System;

namespace FRM.Core.DTOs
{
    public class ProfileViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // Можно добавить дату регистрации, количество постов и т.д.
    }
}