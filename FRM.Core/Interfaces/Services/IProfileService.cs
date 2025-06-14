// FRM.Core.Interfaces.Services/IProfileService.cs
using System;
using System.Threading.Tasks;
using System.Web; // <-- ВОТ ЭТА СТРОКА РЕШИТ ПРОБЛЕМУ
using FRM.Core.DTOs; // <-- Также убедись, что DTO подключены

namespace FRM.Core.Interfaces.Services
{
    public interface IProfileService
    {
        Task<ProfileViewDto> GetUserProfileAsync(Guid userId);
        Task<ProfileEditDto> GetUserProfileForEditAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(Guid userId, ProfileEditDto dto);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task<bool> UpdateProfilePictureAsync(Guid userId, HttpPostedFileBase uploadedFile);
    }
}