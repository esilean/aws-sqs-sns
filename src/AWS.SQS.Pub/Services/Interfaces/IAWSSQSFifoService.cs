using AWS.SQS.Pub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Services.Interfaces
{
    public interface IAWSSQSFifoService
    {
        Task<List<AllMessage>> GetAllMessagesAsync();
        Task<bool> PostMessageAsync(User user);
        Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage);
    }
}
