// FRM.BuisnessLogic.Services/LikeService.cs

// --- НЕОБХОДИМЫЕ USING ДИРЕКТИВЫ ---
using FRM.Core.Entities;
using FRM.Core.Interfaces.Services;
using FRM.Domain;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

// --- ОБЪЯВЛЕНИЕ NAMESPACE ---
namespace FRM.BuisnessLogic.Services
{
    public class LikeService : ILikeService
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> ToggleLikeAsync(Guid commentId, Guid userId)
        {
            // Проверяем, существует ли уже лайк от этого пользователя этому комментарию
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

            if (existingLike != null)
            {
                // Если лайк есть - удаляем его
                _context.Likes.Remove(existingLike);
            }
            else
            {
                // Если лайка нет - создаем новый
                var newLike = new LikeEf { CommentId = commentId, UserId = userId };
                _context.Likes.Add(newLike);
            }

            await _context.SaveChangesAsync();

            // Возвращаем новое количество лайков у комментария
            return await _context.Likes.CountAsync(l => l.CommentId == commentId);
        }
    }
}