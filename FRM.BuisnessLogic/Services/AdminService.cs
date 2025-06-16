using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using FRM.Core.Entities; // <-- Добавьте этот using

namespace FRM.BuisnessLogic.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserAdminViewDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsBanned = u.IsBanned
            }).ToList();
        }

        public async Task BanUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null && user.Role != Core.Enums.UserRole.Admin) // Не баним админов
            {
                user.IsBanned = true;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task UnbanUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsBanned = false;
                await _userRepository.UpdateAsync(user);
            }
        }
        public async Task<UserProfileViewDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null; // Пользователь не найден
            }

            // Маппим сущность в DTO
            return new UserProfileViewDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsBanned = user.IsBanned,
                AgreeToTerms = user.AgreeToTerms
            };
        }
    }
}
