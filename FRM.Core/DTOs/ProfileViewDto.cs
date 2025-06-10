using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FRM.Core.DTOs/ProfileViewDto.cs

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
