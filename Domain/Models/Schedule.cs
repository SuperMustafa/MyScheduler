using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace Domain.Models
{
    public class Schedule
    {
        // Parameterless constructor for EF Core
        public Schedule()
        {
            Days = new List<string>();
            DeviceSettings = new List<DeviceSetting>();
        }


        public int Id { get; set; }
        public string TenantId { get; set; }
        public string CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public TimeSpan Time { get; set; }
        public string Building { get; set; }
        public string TimeZone { get; set; }
        public List<string> Days { get; set; }
        public List<DeviceSetting>? DeviceSettings { get; set; }

        // Parameterized constructor for manual creation
        public Schedule(string tenantId, string customerId, string? name, string? description, bool active, TimeSpan time, string building, string timeZone, List<string> days)
        {

            TenantId = tenantId;
            CustomerId = customerId;
            Name = name;
            Description = description;
            Active = active;
            Time = time;
            Building = building;
            TimeZone = timeZone;
            Days = days ?? new List<string>();
            DeviceSettings = new List<DeviceSetting>();
        }

    }


}
