using System;
using System.Collections.Generic;
using System.Data.Entity; // Добавьте это пространство имен
using System.Linq;
using System.Threading.Tasks;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;

namespace FRM.Domain.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(CommentEf comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentEf>> GetByThreadIdAsync(Guid threadId)
        {
            return await _context.Comments
                .Include(c => c.Author) // Исправлено: правильный синтаксис лямбда-выражения
                .Where(c => c.ThreadId == threadId) // Исправлено: правильный синтаксис Where
                .OrderBy(c => c.CreatedAt) // Исправлено: правильный синтаксис OrderBy
                .ToListAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var commentToDelete = await _context.Comments
                .Include(c => c.Replies) // Загружаем дочерние комментарии
                .FirstOrDefaultAsync(c => c.Id == id);

            if (commentToDelete != null)
            {
                // Вызываем рекурсивный метод удаления
                RemoveCommentAndReplies(commentToDelete);
                await _context.SaveChangesAsync();
            }
        }

        // --- ДОБАВЬ ЭТОТ РЕКУРСИВНЫЙ МЕТОД ---
        private void RemoveCommentAndReplies(CommentEf comment)
        {
            // Сначала рекурсивно удаляем все дочерние комментарии
            // Важно работать с копией коллекции, так как она будет изменяться
            var replies = comment.Replies.ToList();
            foreach (var reply in replies)
            {
                RemoveCommentAndReplies(reply); // Рекурсивный вызов
            }

            // После того как все "дети" удалены, удаляем сам комментарий
            _context.Comments.Remove(comment);
        }   
        public async Task<CommentEf> GetByIdAsync(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}