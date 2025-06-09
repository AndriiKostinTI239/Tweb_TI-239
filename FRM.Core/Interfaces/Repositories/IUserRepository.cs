// IUserRepository.cs
using System.Threading.Tasks;
using FRM.Core.Entities;


namespace FRM.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEf> GetByEmailAsync(string email);
        Task CreateAsync(UserEf user);
    }
}