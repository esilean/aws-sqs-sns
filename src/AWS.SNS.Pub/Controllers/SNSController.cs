using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWS.SNS.Pub.Configs;
using AWS.SNS.Pub.Models;
using AWS.SNS.Pub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace AWS.SNS.Pub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SNSController : ControllerBase
    {
        private readonly IAWSSNSService _AWSSNSService;

        public SNSController(IAWSSNSService AWSSNSService)
        {
            _AWSSNSService = AWSSNSService;
        }

        [HttpPost]
        public async Task<IActionResult> PostSNS([FromBody] User user)
        {
            await _AWSSNSService.PostMessageAsync(user);
            return Ok();
        }
    }
}
