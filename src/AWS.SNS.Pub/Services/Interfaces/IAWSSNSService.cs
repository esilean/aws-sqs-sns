using AWS.SNS.Pub.Models;
using System.Threading.Tasks;

namespace AWS.SNS.Pub.Services.Interfaces
{
    public interface IAWSSNSService
    {
        Task<bool> PostMessageAsync(User user);
    }
}
