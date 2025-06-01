using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Services
{
    public interface IExecutorService
    {
        Task ExecuteScheduleAsync(Schedule schedule);
        Task SendAttributeToDeviceAsync(string deviceId, Dictionary<string, object> attributes, string token);
        Task<string?> GetJwtTokenAsync(string name, string pass);
        Task<string?> GetTenantUserEmailAsync(string tenantId, string token);

    }
}
