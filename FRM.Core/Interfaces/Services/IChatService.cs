using FRM.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRM.Core.Interfaces
{
    public interface IChatService
    {
        Task SaveMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessageHistoryAsync(int count = 50);
    }
}