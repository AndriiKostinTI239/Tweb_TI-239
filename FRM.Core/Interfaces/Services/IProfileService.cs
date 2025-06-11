// FRM.Core.Interfaces.Services/IProfileService.cs
using FRM.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace FRM.Core.Interfaces.Services
{
    public interface IProfileService
    {
        Task<ProfileViewDto> GetUserProfileAsync(Guid userId);
        Task<ProfileEditDto> GetUserProfileForEditAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(Guid userId, ProfileEditDto dto);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    }
}