using Amazon.SQS.Model;
using AWS.SQS.Pub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Helpers.Interfaces
{
    public interface IAWSSQSFifoHelper
    {
        Task<List<Message>> ReceiveMessageAsync();
        Task<bool> SendMessageAsync(UserDetail userDetail);
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
    }
}
