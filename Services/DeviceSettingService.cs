using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistance.Repositories;

namespace Services
{
    public class DeviceSettingService : IDeviceSettingService
    {
        private readonly IDeviceSettingRepository _repository;

        public DeviceSettingService(IDeviceSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeviceSetting?> GetByDeviceIdAndNameAsync(String deviceId, string name)
        {
            return await _repository.GetByDeviceIdAndNameAsync(deviceId, name);
        }

        public async Task AddAsync(DeviceSetting setting)
        {
            await _repository.AddAsync(setting);
        }
    }

}
