using AWS.SQS.Pub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Services.Interfaces
{
    public interface IAWSSQSService
    {
        Task<List<AllMessage>> GetAllMessagesAsync();
        Task<List<AllMessage>> GetAllMessagesDLQAsync();
        Task<bool> PostMessageAsync(User user);
        Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage);
    }
}
