using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRM.Core.DTOs;

namespace FRM.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync();
        Task BanUserAsync(Guid userId);
        Task UnbanUserAsync(Guid userId);
        Task<UserProfileViewDto> GetUserProfileAsync(Guid userId);
    }
}