using Fischless.Configuration;
using System.Reflection;

namespace TiktokLiveRec;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<string> Theme { get; } = new(nameof(Theme), string.Empty);
    public static ConfigurationDefinition<Room[]> Rooms { get; } = new(nameof(Rooms), []);

    public static ConfigurationDefinition<bool> IsToNotify { get; } = new(nameof(IsToNotify), true);
    public static ConfigurationDefinition<bool> IsToSpider { get; } = new(nameof(IsToSpider), true);
    public static ConfigurationDefinition<bool> IsToRecord { get; } = new(nameof(IsToRecord), true);
}

public class Room
{
    public string NickName { get; set; } = null!;
    public string RoomUrl { get; set; } = null!;
    public bool IsToNotify { get; set; } = true;
    public bool IsToSpider { get; set; } = true;
    public bool IsToRecord { get; set; } = true;

    public override string ToString() => $"{RoomUrl},{NickName}";
}
