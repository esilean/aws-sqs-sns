using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.SQS.Pub.Configs;
using AWS.SQS.Pub.Helpers.Interfaces;
using AWS.SQS.Pub.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Helpers
{
    public class AWSSQSFifoHelper : IAWSSQSFifoHelper
    {

        private readonly IAmazonSQS _sqs;
        private readonly ServiceConfiguration _settings;
        public AWSSQSFifoHelper(
           IAmazonSQS sqs,
           IOptions<ServiceConfiguration> settings)
        {
            _sqs = sqs;
            _settings = settings.Value;


        }

        public async Task<List<Message>> ReceiveMessageAsync()
        {

            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.AWSSQS.FifoQueueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5,
                };
                //CheckIs there any new message available to process  
                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendMessageAsync(UserDetail userDetail)
        {
            try
            {
                string message = JsonConvert.SerializeObject(userDetail);
                var sendRequest = new SendMessageRequest(_settings.AWSSQS.FifoQueueUrl, message)
                {
                    MessageGroupId = userDetail.Id.ToString()
                };

                // Post message or payload to queue  
                var sendResult = await _sqs.SendMessageAsync(sendRequest);

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync(_settings.AWSSQS.FifoQueueUrl, messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
