using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistance.Repositories;

namespace Services
{
    public class DeviceAttributeService : IDeviceAttributeService
    {
        private readonly IDeviceAttributeRepository _repository;

        public DeviceAttributeService(IDeviceAttributeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExistsAsync(int deviceSettingId, string key, string value)
        {
            return await _repository.ExistsAsync(deviceSettingId, key, value);
        }

        public async Task AddAsync(DeviceAttribute attribute)
        {
            await _repository.AddAsync(attribute);
        }
    }

}
