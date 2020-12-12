using AWS.SQS.Pub.Helpers.Interfaces;
using AWS.SQS.Pub.Models;
using AWS.SQS.Pub.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Services
{
    public class AWSSQSFifoService : IAWSSQSFifoService
    {

        private readonly IAWSSQSFifoHelper _AWSSQSFifoHelper;
        public AWSSQSFifoService(IAWSSQSFifoHelper AWSSQSFifoHelper)
        {
            _AWSSQSFifoHelper = AWSSQSFifoHelper;
        }

        public async Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            try
            {
                return await _AWSSQSFifoHelper.DeleteMessageAsync(deleteMessage.ReceiptHandle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AllMessage>> GetAllMessagesAsync()
        {
            var allMessages = new List<AllMessage>();
            try
            {
                var messages = await _AWSSQSFifoHelper.ReceiveMessageAsync();

                allMessages = messages.Select(c => new AllMessage
                {
                    MessageId = c.MessageId,
                    ReceiptHandle = c.ReceiptHandle,
                    UserDetail = JsonConvert.DeserializeObject<UserDetail>(c.Body)
                }).ToList();

                return allMessages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostMessageAsync(User user)
        {
            try
            {
                UserDetail userDetail = new UserDetail
                {
                    Id = new Random().Next(999999999),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    EmailId = user.EmailId,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };

                return await _AWSSQSFifoHelper.SendMessageAsync(userDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
