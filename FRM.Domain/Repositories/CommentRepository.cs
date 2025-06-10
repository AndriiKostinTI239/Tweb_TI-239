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
    }
}