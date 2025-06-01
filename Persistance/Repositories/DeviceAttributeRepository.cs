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
    public class DeviceAttributeRepository : IDeviceAttributeRepository
    {
        private readonly AppDbContext _context;

        public DeviceAttributeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int deviceSettingId, string key, string value)
        {
            return await _context.DeviceAttributes
                .AnyAsync(attr =>
                    attr.DeviceSettingId == deviceSettingId &&
                    attr.Key == key &&
                    attr.Value == value);
        }

        public async Task AddAsync(DeviceAttribute attribute)
        {
            await _context.DeviceAttributes.AddAsync(attribute);
            await _context.SaveChangesAsync();
        }
    }

}
