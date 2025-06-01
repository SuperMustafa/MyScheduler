using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class schedualeDto
    {
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
        public List<DeviceSettingDto> Settings { get; set; }
    }
}
