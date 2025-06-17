// FRM.Domain.Repositories/ThreadRepository.cs
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FRM.Domain.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly AppDbContext _context;

        public ThreadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ThreadEf thread)
        {
            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ThreadEf>> GetAllAsync()
        {
            return await _context.Threads
                .Include(t => t.Author)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<ThreadEf> GetByIdAsync(Guid id)
        {
            return await _context.Threads
                .Include(t => t.Author)
                .Include(t => t.Comments.Select(c => c.Author))
                .Include(t => t.Comments.Select(c => c.Likes))
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var thread = await _context.Threads.FindAsync(id);
            if (thread != null)
            {
                _context.Threads.Remove(thread);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ThreadEf>> GetThreadsByAuthorIdAsync(Guid authorId)
        {
            return await _context.Threads
                .Where(t => t.AuthorId == authorId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountThreadsByAuthorInLastWeekAsync(Guid authorId)
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            return await _context.Threads
                .CountAsync(t => t.AuthorId == authorId && t.CreatedAt >= oneWeekAgo);
        }

        public async Task UpdateAsync(ThreadEf thread)
        {
            _context.Entry(thread).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
   
    }
}