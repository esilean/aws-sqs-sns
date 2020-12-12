using AWS.SQS.Pub.Models;
using AWS.SQS.Pub.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Workers
{
    public class AWSSQSFifoBWorker : BackgroundService
    {
        private readonly ILogger<AWSSQSFifoBWorker> _logger;
        private readonly IAWSSQSFifoService _AWSSQSFifoService;

        public AWSSQSFifoBWorker(
            ILogger<AWSSQSFifoBWorker> logger,
            IAWSSQSFifoService AWSSQSFifoService)
        {
            _logger = logger;
            _AWSSQSFifoService = AWSSQSFifoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogCritical("Hosted service B starting...");

            // loop until a cancalation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogCritical("Hosted service B executing - {0}", DateTime.Now);

                try
                {
                    var allMessages = await _AWSSQSFifoService.GetAllMessagesAsync();

                    foreach (var message in allMessages)
                    {
                        _logger.LogCritical("B Processing message: {messageId} | {time}", message.MessageId, DateTime.Now);

                        await _AWSSQSFifoService.DeleteMessageAsync(new DeleteMessage
                        {
                            ReceiptHandle = message.ReceiptHandle
                        });
                        _logger.LogCritical("B Deleting message: {messageId} | {time}", message.MessageId, DateTime.Now);
                    }

                    // wait for 2 seconds
                    //await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (OperationCanceledException) { }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("Stopping worker...");
            return Task.CompletedTask;
        }

    }
}
