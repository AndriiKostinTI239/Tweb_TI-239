using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRM.Core.Entities;

namespace FRM.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(CommentEf comment);
        Task<IEnumerable<CommentEf>> GetByThreadIdAsync(Guid threadId);
        Task DeleteAsync(Guid id);
        Task<CommentEf> GetByIdAsync(Guid id);
    }
}