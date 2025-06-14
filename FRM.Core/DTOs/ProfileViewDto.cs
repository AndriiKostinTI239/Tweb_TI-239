using System.Collections.Generic;
using System;

namespace FRM.Core.DTOs
{
    public class ProfileViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // Можно добавить дату регистрации, количество постов и т.д.
        public string ProfilePictureUrl { get; set; }
        public List<ThreadDto> UserThreads { get; set; } = new List<ThreadDto>();
    }
}