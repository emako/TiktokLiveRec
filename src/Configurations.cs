using Fischless.Configuration;
using System.Reflection;

namespace TiktokLiveRec;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<string> Theme { get; } = new(nameof(Theme), string.Empty);
    public static ConfigurationDefinition<Room[]> Rooms { get; } = new(nameof(Rooms), []);
    public static ConfigurationDefinition<bool> IsUseStatusTray { get; } = new(nameof(IsUseStatusTray), true);
    public static ConfigurationDefinition<bool> IsToNotify { get; } = new(nameof(IsToNotify), true);
    public static ConfigurationDefinition<bool> IsToRecord { get; } = new(nameof(IsToRecord), true);
    public static ConfigurationDefinition<string> RecordFormat { get; } = new(nameof(RecordFormat), "TS");
    public static ConfigurationDefinition<bool> IsRemoveTs { get; } = new(nameof(IsRemoveTs), false);
    public static ConfigurationDefinition<string> SaveFolder { get; } = new(nameof(SaveFolder), string.Empty);
    public static ConfigurationDefinition<string> Player { get; } = new(nameof(Player), "FFplay");
    public static ConfigurationDefinition<bool> IsPlayerRect { get; } = new(nameof(IsPlayerRect), false);
    public static ConfigurationDefinition<bool> IsUseProxy { get; } = new(nameof(IsUseProxy), false);
    public static ConfigurationDefinition<string> ProxyUrl { get; } = new(nameof(ProxyUrl), string.Empty);
    public static ConfigurationDefinition<string> Cookie { get; } = new(nameof(Cookie), string.Empty);
}

public class Room
{
    public string NickName { get; set; } = null!;
    public string RoomUrl { get; set; } = null!;
    public bool IsToNotify { get; set; } = true;
    public bool IsToRecord { get; set; } = true;

    public override string ToString() => $"{RoomUrl},{NickName}";
}
