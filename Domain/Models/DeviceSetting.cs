using System.Text.Json.Serialization;
using Domain.Models;

public class DeviceSetting
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DeviceId { get; set; } // this deals with device id at thingsboard

    public int ScheduleId { get; set; }
    
    public Schedule Schedule { get; set; }

    public List<DeviceAttribute> Attributes { get; set; } = new();
}
