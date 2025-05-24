using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDeviceAttributeService
    {
        Task<bool> ExistsAsync(int deviceSettingId, string key, string value);
        Task AddAsync(DeviceAttribute attribute);
    }

}
