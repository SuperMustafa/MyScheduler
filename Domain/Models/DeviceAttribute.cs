public class DeviceAttribute
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;

    public int DeviceSettingId { get; set; }
    public DeviceSetting DeviceSetting { get; set; } = null!;
}
