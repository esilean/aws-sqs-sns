using Amazon.SQS.Model;
using AWS.SQS.Pub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Helpers.Interfaces
{
    public interface IAWSSQSHelper
    {
        Task<List<Message>> ReceiveMessageAsync();
        Task<List<Message>> ReceiveMessageDLQAsync();

        Task<bool> SendMessageAsync(UserDetail userDetail);
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
    }
}
