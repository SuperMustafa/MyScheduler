using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Persistance.Repositories
{
    public interface IDeviceAttributeRepository
    {
        Task<bool> ExistsAsync(int deviceSettingId, string key, string value);
        Task AddAsync(DeviceAttribute attribute);
    }
}
