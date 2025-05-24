using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Dtos
{
    public class DeviceSettingDto
    {
        public string Name { get; set; }
        public string DeviceId { get; set; }
        public List<DeviceAttributeDto> Attributes { get; set; }
    }
}
