// IUserRepository.cs
using FRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task<UserEf> GetByIdAsync(Guid id); // Добавляем этот метод
        Task CreateAsync(UserEf user);
        Task UpdateAsync(UserEf user);
        Task<IEnumerable<UserEf>> GetAllAsync();

}
