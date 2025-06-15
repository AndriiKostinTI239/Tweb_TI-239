using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRM.Core.Entities;

namespace FRM.Core.Interfaces.Repositories
{
    public interface IThreadRepository
    {
        Task<ThreadEf> GetByIdAsync(Guid id);
        Task<IEnumerable<ThreadEf>> GetAllAsync();
        Task CreateAsync(ThreadEf thread);
        Task<IEnumerable<ThreadEf>> GetThreadsByAuthorIdAsync(Guid authorId);
        Task DeleteAsync(Guid id);
        Task<int> CountThreadsByAuthorInLastWeekAsync(Guid authorId);
    }
}