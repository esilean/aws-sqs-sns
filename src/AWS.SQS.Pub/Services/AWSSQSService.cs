using Amazon.SQS.Model;
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
    public class AWSSQSService : IAWSSQSService
    {

        private readonly IAWSSQSHelper _AWSSQSHelper;
        public AWSSQSService(IAWSSQSHelper AWSSQSHelper)
        {
            _AWSSQSHelper = AWSSQSHelper;
        }

        public async Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            try
            {
                return await _AWSSQSHelper.DeleteMessageAsync(deleteMessage.ReceiptHandle);
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
                var messages = await _AWSSQSHelper.ReceiveMessageAsync();

                //polling from SNS > SQS
                //allMessages = messages.Select(c => new AllMessage
                //{
                //    MessageId = c.MessageId,
                //    ReceiptHandle = c.ReceiptHandle,
                //    UserDetail = JsonConvert.DeserializeObject<UserDetail>(JsonConvert.DeserializeObject<SNSMessage>(c.Body).Message)
                //}).ToList();

                //polling from SQS direct
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

        public async Task<List<AllMessage>> GetAllMessagesDLQAsync()
        {
            var allMessages = new List<AllMessage>();
            try
            {
                var messages = await _AWSSQSHelper.ReceiveMessageDLQAsync();

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

                return await _AWSSQSHelper.SendMessageAsync(userDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
