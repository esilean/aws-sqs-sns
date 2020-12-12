using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWS.SNS.Pub.Configs;
using AWS.SNS.Pub.Models;
using AWS.SNS.Pub.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AWS.SNS.Pub.Services
{
    public class AWSSNSService : IAWSSNSService
    {

        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly ServiceConfiguration _settings;

        public AWSSNSService(
            IAmazonSimpleNotificationService amazonSimpleNotificationService,
            IOptions<ServiceConfiguration> settings)
        {
            _settings = settings.Value;
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
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

                string message = JsonConvert.SerializeObject(userDetail);
                var request = new PublishRequest
                {
                    Message = message,
                    TopicArn = _settings.AWSSNS.TopicARN,
                };

                var response = await _amazonSimpleNotificationService.PublishAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
