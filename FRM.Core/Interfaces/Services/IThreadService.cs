using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRM.Core.DTOs;
using FRM.Core.Entities;

public interface IThreadService
{
    Task CreateThreadAsync(CreateThreadDto dto, Guid authorId);
    Task<IEnumerable<ThreadDto>> GetAllThreadsAsync();
    Task<ThreadEf> GetThreadByIdAsync(Guid id);
    Task AddCommentAsync(AddCommentDto dto, Guid threadId, Guid authorId);
}