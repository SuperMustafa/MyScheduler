using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Persistance.Repositories
{
    public interface IDeviceSettingRepository
    {
        Task<DeviceSetting?> GetByDeviceIdAndNameAsync(String deviceId, string name);
        Task AddAsync(DeviceSetting setting);
    }
}
