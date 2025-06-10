using FRM.Core.Entities;
using FRM.Core.Interfaces;
using FRM.Domain; 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FRM.BuisnessLogic.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;

        // Используем Dependency Injection для получения контекста БД
        public ChatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessageHistoryAsync(int count = 50)
        {
            return await _context.Messages
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .OrderBy(m => m.Timestamp) // Возвращаем в хронологическом порядке
                .ToListAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            message.Timestamp = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}