using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Models;
using Persistance.Repositories;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepsitory _scheduleRepository;
        private readonly IDeviceAttributeService _deviceAttributeService;
        private readonly IDeviceSettingService _deviceSettingService;
        public ScheduleService(IScheduleRepsitory repository, IDeviceAttributeService deviceAttributeService, IDeviceSettingService deviceSettingService)
        {
            _scheduleRepository = repository;
            _deviceAttributeService = deviceAttributeService;
            _deviceSettingService = deviceSettingService;
        }

        public async Task<IEnumerable<schedualeDto>> GetAllAsync()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            return schedules.Select(s => new schedualeDto
            {
                Id=s.Id,
                TenantId = s.TenantId,
                CustomerId= s.CustomerId,
                Name = s.Name,
                Description = s.Description,
                Active = s.Active,
                Time = (TimeSpan)s.Time,
                Building = s.Building,
                TimeZone = s.TimeZone,
                Days = s.Days.Select(d => d).ToList(), // Assuming you have Days as List<Day> or similar
                Settings = s.DeviceSettings.Select(ds => new DeviceSettingDto
                {
                    DeviceId = ds.DeviceId,
                    Name = ds.Name,
                    Attributes = ds.Attributes.Select(attr => new DeviceAttributeDto
                    {
                        Key = attr.Key,
                        Value = attr.Value
                    }).ToList()
                }).ToList()
            });
        }

        public async Task<schedualeDto?> GetByIdAsync(int id)
        {
            var s = await _scheduleRepository.GetByIdAsync(id);
            if (s == null) return null;

            return new schedualeDto
            {
                TenantId = s.TenantId,
                CustomerId = s.CustomerId,
                Name = s.Name,
                Description = s.Description,
                Active = s.Active,
                Time = (TimeSpan)s.Time,
                Building = s.Building,
                TimeZone = s.TimeZone,
                Days = s.Days.Select(d => d).ToList(),
                Settings = s.DeviceSettings.Select(ds => new DeviceSettingDto
                {
                    DeviceId = ds.DeviceId,
                    Name = ds.Name,
                    Attributes = ds.Attributes.Select(attr => new DeviceAttributeDto
                    {
                        Key = attr.Key,
                        Value = attr.Value
                    }).ToList()
                }).ToList()
            };
        }


        public async Task<Schedule> CreateAsync(schedualeDto dto)
        {
            var schedule = new Schedule( dto.TenantId, dto.CustomerId, dto.Name, dto.Description, dto.Active, dto.Time, dto.Building, dto.TimeZone, dto.Days);
            await _scheduleRepository.AddAsync(schedule); 

            foreach (var settingDto in dto.Settings ?? new List<DeviceSettingDto>())
            {
                var attributes = settingDto.Attributes ?? new List<DeviceAttributeDto>();

                var existingDeviceSetting = await _deviceSettingService
                    .GetByDeviceIdAndNameAsync(settingDto.DeviceId, settingDto.Name);

                DeviceSetting deviceSetting;

                    deviceSetting = new DeviceSetting
                    {
                        Name = settingDto.Name,
                        DeviceId = settingDto.DeviceId,
                        ScheduleId = schedule.Id
                    };

                    await _deviceSettingService.AddAsync(deviceSetting);
                

                foreach (var attrDto in attributes)
                {
                    var exists = await _deviceAttributeService
                        .ExistsAsync(deviceSetting.Id, attrDto.Key, attrDto.Value);

                    // if (!exists)
                    {
                        var attribute = new DeviceAttribute
                        {
                            Key = attrDto.Key,
                            Value = attrDto.Value,
                            DeviceSettingId = deviceSetting.Id
                        };

                        await _deviceAttributeService.AddAsync(attribute);
                    }
                }
            }

            return schedule;
        }
        public async Task<List<Schedule>> GetSchedulesByCustomerIdAsync(string customerId)
        {
            return await _scheduleRepository.GetSchedulesByCustomerIdAsync(customerId);
        }

        public async Task<List<Schedule>> GetSchedulesByTenantIdAsync(string tenantId)
        {
            return await _scheduleRepository.GetSchedulesByTenantIdAsync(tenantId);
        }







        
        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return false;

            return await _scheduleRepository.DeleteWithChildrenAsync(schedule);
        }



    }
}
