// FRM.BuisnessLogic.Services/LayoutService.cs
using FRM.Core.DTOs;
using FRM.Core.Interfaces.Repositories;
using FRM.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace FRM.BuisnessLogic.Services
{
    // Класс реализует ILayoutService
    public class LayoutService : ILayoutService
    {
        private readonly IUserRepository _userRepo;
        private readonly IThreadRepository _threadRepo;

        public LayoutService(IUserRepository userRepo, IThreadRepository threadRepo)
        {
            _userRepo = userRepo;
            _threadRepo = threadRepo;
        }

        // Реализация СИНХРОННОГО метода из интерфейса
        public LayoutUserDto GetLayoutUserData(Guid userId)
        {
            if (userId == Guid.Empty) return null;

            // Используем Task.Run, чтобы избежать дэдлока
            return Task.Run(() => GetLayoutUserDataInternalAsync(userId)).Result;
        }

        // Приватный асинхронный метод, который содержит всю логику
        private async Task<LayoutUserDto> GetLayoutUserDataInternalAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return null;

            var threadsInLastWeek = await _threadRepo.CountThreadsByAuthorInLastWeekAsync(userId);
            var rating = CalculateRating(threadsInLastWeek);

            return new LayoutUserDto
            {
                Nickname = user.Name,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Rating = rating
            };
        }

        private string CalculateRating(int threadCount)
        {
            if (threadCount > 15) return "Бубный";
            if (threadCount > 5) return "Гик";
            return "Хилый";
        }
    }
}