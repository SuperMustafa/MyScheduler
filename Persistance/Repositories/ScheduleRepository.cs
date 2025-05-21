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
    public class ScheduleRepository : IScheduleRepsitory


    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Schedules
                .Include(s => s.DeviceSettings)
                    .ThenInclude(ds => ds.Attributes)
                .ToListAsync();
        }


        public async Task<Schedule?> GetByIdAsync(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.DeviceSettings)
                    .ThenInclude(d => d.Attributes)
                .FirstOrDefaultAsync(s => s.Id == id);
            return schedule;

        }
           

        public async Task AddAsync(Schedule schedule)
        {
             await _context.Schedules.AddAsync(schedule);
            _context.SaveChanges();
        }

        public async Task<List<Schedule>> GetSchedulesByCustomerIdAsync(string customerId)
        {
            return await _context.Schedules
                  .Include(s => s.DeviceSettings)
                  .ThenInclude(ds => ds.Attributes)
                  .Where(s => s.CustomerId == customerId)
                  .ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesByTenantIdAsync(string tenantId)
        {
            return await _context.Schedules
          .Include(s => s.DeviceSettings)
          .ThenInclude(ds => ds.Attributes)
          .Where(s => s.TenantId == tenantId)
          .ToListAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return false;

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWithChildrenAsync(Schedule schedule)
        {
            if (schedule.DeviceSettings != null)
            {
                foreach (var setting in schedule.DeviceSettings)
                {
                    if (setting.Attributes != null)
                        _context.DeviceAttributes.RemoveRange(setting.Attributes);

                    _context.DeviceSettings.Remove(setting);
                }
            }

            _context.Schedules.Remove(schedule);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}

