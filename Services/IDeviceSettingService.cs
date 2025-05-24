using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDeviceSettingService
    {
        Task<DeviceSetting?> GetByDeviceIdAndNameAsync(String deviceId, string name);
        Task AddAsync(DeviceSetting setting);
    }

}
