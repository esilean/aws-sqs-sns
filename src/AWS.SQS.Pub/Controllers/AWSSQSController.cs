using AWS.SQS.Pub.Models;
using AWS.SQS.Pub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AWS.SQS.Pub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AWSSQSController : ControllerBase
    {

        private readonly IAWSSQSService _AWSSQSService;
        private readonly IAWSSQSFifoService _AWSSQSFifoService;

        public AWSSQSController(IAWSSQSService AWSSQSService, IAWSSQSFifoService AWSSQSFifoService)
        {
            _AWSSQSService = AWSSQSService;
            _AWSSQSFifoService = AWSSQSFifoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessagesAsync()
        {
            var result = await _AWSSQSService.GetAllMessagesAsync();
            return Ok(result);
        }

        [HttpGet("dlq")]
        public async Task<IActionResult> GetAllMessagesDLQAsync()
        {
            var result = await _AWSSQSService.GetAllMessagesDLQAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessageAsync([FromBody] User user)
        {
            var result = await _AWSSQSService.PostMessageAsync(user);
            return Ok(new { isSucess = result });
        }

        [HttpPost("fifo")]
        public async Task<IActionResult> PostMessageFifoAsync([FromBody] User user)
        {
            var result = await _AWSSQSFifoService.PostMessageAsync(user);
            return Ok(new { isSucess = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            var result = await _AWSSQSService.DeleteMessageAsync(deleteMessage);
            return Ok(new { isSucess = result });
        }
    }
}
