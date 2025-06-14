// FRM.BuisnessLogic.Services/ProfileService.cs
using FRM.BuisnessLogic.Helper;
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace FRM.BuisnessLogic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepo;
        private readonly Hasher _hasher;
        private readonly IThreadRepository _threadRepo;

        // Конструктор для DI (если будешь использовать) и для ручного создания
        public ProfileService(IUserRepository userRepo, IThreadRepository threadRepo, Hasher hasher)
        {
            _userRepo = userRepo;
            _threadRepo = threadRepo; // Теперь это будет работать
            _hasher = hasher;
        }

        public async Task<ProfileViewDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return null;
            var userThreads = await _threadRepo.GetThreadsByAuthorIdAsync(userId);

            var threadDtos = userThreads.Select(t => new ThreadDto
            {
                Id = t.Id,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
                // Здесь нам не нужен AuthorName, так как мы уже на странице автора
                // CommentCount тоже можно опустить для простоты или посчитать, если нужно
                CommentCount = t.Comments?.Count ?? 0
            }).ToList();

            return new ProfileViewDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                UserThreads = threadDtos
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
        public async Task<bool> UpdateProfilePictureAsync(Guid userId, HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.ContentLength == 0)
                return false;

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            // 1. Определяем путь для сохранения
            var uploadsFolder = "~/Uploads/ProfilePictures";
            var physicalFolder = HttpContext.Current.Server.MapPath(uploadsFolder);

            // Создаем папку, если ее нет
            if (!Directory.Exists(physicalFolder))
            {
                Directory.CreateDirectory(physicalFolder);
            }

            // 2. Удаляем старый файл, если он был
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                var oldFilePath = HttpContext.Current.Server.MapPath(user.ProfilePictureUrl);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            // 3. Создаем уникальное имя файла, чтобы избежать конфликтов
            var fileExtension = Path.GetExtension(uploadedFile.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var newFilePath = Path.Combine(physicalFolder, uniqueFileName);

            // 4. Сохраняем новый файл
            uploadedFile.SaveAs(newFilePath);

            // 5. Обновляем путь в базе данных
            user.ProfilePictureUrl = $"{uploadsFolder}/{uniqueFileName}";
            await _userRepo.UpdateAsync(user);

            return true;
        }
        }
}