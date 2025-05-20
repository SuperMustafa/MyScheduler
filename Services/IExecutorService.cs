using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Services
{
    public interface IExecutorService
    {
        Task ExecuteScheduleAsync(Schedule schedule);
        Task SendAttributeToDeviceAsync(string deviceId, Dictionary<string, object> attributes, string token);
        Task<string?> GetJwtTokenAsync();
    }
}
