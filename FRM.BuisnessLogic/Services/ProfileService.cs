// FRM.BuisnessLogic.Services/ProfileService.cs
using FRM.BuisnessLogic.Helper;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace FRM.BuisnessLogic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepo;
        private readonly Hasher _hasher;

        // Конструктор для DI (если будешь использовать) и для ручного создания
        public ProfileService(IUserRepository userRepo, Hasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        public async Task<ProfileViewDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return null;

            return new ProfileViewDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<ProfileEditDto> GetUserProfileForEditAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return null;

            return new ProfileEditDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> UpdateUserProfileAsync(Guid userId, ProfileEditDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            // Проверка, не занят ли новый email другим пользователем
            var existingUserWithEmail = await _userRepo.GetByEmailAsync(dto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
            {
                // Можно выбрасывать исключение или возвращать специальный результат
                return false;
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            // Проверяем, совпадает ли старый пароль
            if (!_hasher.VerifyPassword(dto.OldPassword, user.HashPassword))
            {
                return false;
            }

            // Хешируем и сохраняем новый пароль
            user.HashPassword = _hasher.HashPassword(dto.NewPassword);
            await _userRepo.UpdateAsync(user);

            return true;
        }
    }
}