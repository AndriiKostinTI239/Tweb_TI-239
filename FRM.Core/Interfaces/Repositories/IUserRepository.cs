// IUserRepository.cs
using System;
using System.Threading.Tasks;
using FRM.Core.Entities;



    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task<UserEf> GetByIdAsync(Guid id); // Добавляем этот метод
        Task CreateAsync(UserEf user);
        Task UpdateAsync(UserEf user);
}
