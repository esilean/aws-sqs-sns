using AWS.SQS.Pub.Models;
using AWS.SQS.Pub.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Workers
{
    public class AWSSQSWorker : BackgroundService
    {
        private readonly ILogger<AWSSQSWorker> _logger;
        private readonly IAWSSQSService _AWSSQSService;

        public AWSSQSWorker(
            ILogger<AWSSQSWorker> logger,
            IAWSSQSService AWSSQSService)
        {
            _logger = logger;
            _AWSSQSService = AWSSQSService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning("Hosted service S starting...");

            // loop until a cancalation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning("Hosted service S executing - {0}", DateTime.Now);

                try
                {
                    var allMessages = await _AWSSQSService.GetAllMessagesDLQAsync();

                    foreach (var message in allMessages)
                    {
                        _logger.LogWarning("S Processing message: {messageId} | {time}", message.MessageId, DateTime.Now);
                        _logger.LogWarning("S Message: {message}", message.UserDetail);

                        await _AWSSQSService.DeleteMessageAsync(new DeleteMessage
                        {
                            ReceiptHandle = message.ReceiptHandle
                        });
                    }

                    // wait for 2 seconds
                    //await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (OperationCanceledException) { }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stopping worker...");
            return Task.CompletedTask;
        }

    }
}
