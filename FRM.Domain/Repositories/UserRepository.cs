using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FRM.Core.Entities;
using FRM.Core.Interfaces.Repositories;
using FRM.Domain;

namespace FRM.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        // Конструктор с внедрением зависимости
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEf> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Реализация отсутствующего метода
        public async Task<UserEf> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task CreateAsync(UserEf user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}