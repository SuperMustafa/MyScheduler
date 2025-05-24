using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class DeviceSettingRepository : IDeviceSettingRepository
    {
        private readonly AppDbContext _context;

        public DeviceSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeviceSetting?> GetByDeviceIdAndNameAsync(String deviceId, string name)
        {
            return await _context.DeviceSettings
                .FirstOrDefaultAsync(ds => ds.DeviceId == deviceId && ds.Name == name);
        }

        public async Task AddAsync(DeviceSetting setting)
        {
            await _context.DeviceSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }
    }

}
