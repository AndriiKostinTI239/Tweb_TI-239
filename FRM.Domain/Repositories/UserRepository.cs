// FRM.Domain.Repositories/UserRepository.cs
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using System;
using System.Data.Entity; // Убедись, что используешь правильный using для EF
using System.Threading.Tasks;

namespace FRM.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; // Предполагаем, что у тебя есть DbContext

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Твои существующие методы CreateAsync и GetByEmailAsync...
        public async Task CreateAsync(UserEf user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEf> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // --- НОВАЯ РЕАЛИЗАЦИЯ ---
        public async Task<UserEf> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(UserEf user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}