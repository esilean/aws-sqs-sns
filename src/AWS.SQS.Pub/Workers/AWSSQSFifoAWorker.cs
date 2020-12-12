using AWS.SQS.Pub.Models;
using AWS.SQS.Pub.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Workers
{
    public class AWSSQSFifoAWorker : BackgroundService
    {
        private readonly ILogger<AWSSQSFifoAWorker> _logger;
        private readonly IAWSSQSFifoService _AWSSQSFifoService;

        public AWSSQSFifoAWorker(
            ILogger<AWSSQSFifoAWorker> logger,
            IAWSSQSFifoService AWSSQSFifoService)
        {
            _logger = logger;
            _AWSSQSFifoService = AWSSQSFifoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted service A starting...");

            // loop until a cancalation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Hosted service A executing - {0}", DateTime.Now);

                try
                {
                    var allMessages = await _AWSSQSFifoService.GetAllMessagesAsync();

                    foreach (var message in allMessages)
                    {
                        _logger.LogInformation("A Processing message: {messageId} | {time}", message.MessageId, DateTime.Now);

                        await _AWSSQSFifoService.DeleteMessageAsync(new DeleteMessage
                        {
                            ReceiptHandle = message.ReceiptHandle
                        });
                        _logger.LogInformation("A Deleting message: {messageId} | {time}", message.MessageId, DateTime.Now);
                    }

                    // wait for 2 seconds
                    //await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (OperationCanceledException) { }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping worker...");
            return Task.CompletedTask;
        }

    }
}
