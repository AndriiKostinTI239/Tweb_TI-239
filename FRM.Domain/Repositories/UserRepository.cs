// FRM.Domain/Repositories/UserRepository.cs

using System;
using System.Collections.Generic;
using System.Data.Entity; // <-- Убедитесь, что этот using есть (или Microsoft.EntityFrameworkCore, если вы на Core)
using System.Threading.Tasks;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;

namespace FRM.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        // Конструктор, который получает контекст БД через DI
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- ВАШИ СТАРЫЕ, УЖЕ РЕАЛИЗОВАННЫЕ МЕТОДЫ ---
        public async Task<UserEf> GetByEmailAsync(string email)
        {
            // Ваша реальная реализация
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(UserEf user)
        {
            // Ваша реальная реализация
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }


        // --- НОВЫЕ МЕТОДЫ, КОТОРЫЕ НУЖНО РЕАЛИЗОВАТЬ ---

        public async Task<IEnumerable<UserEf>> GetAllAsync()
        {
            // Заменяем заглушку на реальный код
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEf> GetByIdAsync(Guid id)
        {
            // Заменяем заглушку на реальный код
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(UserEf user)
        {
            // Заменяем заглушку на реальный код
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}