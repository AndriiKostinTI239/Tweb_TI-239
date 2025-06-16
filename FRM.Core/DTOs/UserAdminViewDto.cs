using System;
using FRM.Core.Enums;

namespace FRM.Core.DTOs
{
    public class UserAdminViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public bool IsBanned { get; set; }
    }
}