using Fischless.Configuration;
using System.Reflection;

namespace TiktokLiveRec;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<string> Language { get; } = new(nameof(Language), string.Empty);
    public static ConfigurationDefinition<string> Theme { get; } = new(nameof(Theme), string.Empty);
    public static ConfigurationDefinition<bool> IsOffRemindCloseToTray { get; } = new(nameof(IsOffRemindCloseToTray), false);
    public static ConfigurationDefinition<Room[]> Rooms { get; } = new(nameof(Rooms), []);
    public static ConfigurationDefinition<bool> IsUseStatusTray { get; } = new(nameof(IsUseStatusTray), true);
    public static ConfigurationDefinition<int> RoutineInterval { get; } = new(nameof(RoutineInterval), 30000);
    public static ConfigurationDefinition<bool> IsToNotify { get; } = new(nameof(IsToNotify), true);
    public static ConfigurationDefinition<bool> IsToNotifyWithSystem { get; } = new(nameof(IsToNotifyWithSystem), true);
    public static ConfigurationDefinition<bool> IsToNotifyWithMusic { get; } = new(nameof(IsToNotifyWithMusic), true);
    public static ConfigurationDefinition<string?> ToNotifyWithMusicPath { get; } = new(nameof(ToNotifyWithMusicPath), null);
    public static ConfigurationDefinition<bool> IsToNotifyWithEmail { get; } = new(nameof(IsToNotifyWithEmail), true);
    public static ConfigurationDefinition<string> ToNotifyWithEmailSmtp { get; } = new(nameof(ToNotifyWithEmailSmtp), null!);
    public static ConfigurationDefinition<string> ToNotifyWithEmailUserName { get; } = new(nameof(ToNotifyWithEmailUserName), null!);
    public static ConfigurationDefinition<string> ToNotifyWithEmailPassword { get; } = new(nameof(ToNotifyWithEmailPassword), null!);
    public static ConfigurationDefinition<bool> IsToNotifyGotoRoomUrl { get; } = new(nameof(IsToNotifyGotoRoomUrl), false);
    public static ConfigurationDefinition<bool> IsToNotifyGotoRoomUrlAndMute { get; } = new(nameof(IsToNotifyGotoRoomUrlAndMute), false);
    public static ConfigurationDefinition<bool> IsToRecord { get; } = new(nameof(IsToRecord), true);
    public static ConfigurationDefinition<string> RecordFormat { get; } = new(nameof(RecordFormat), "TS");
    public static ConfigurationDefinition<bool> IsRemoveTs { get; } = new(nameof(IsRemoveTs), false);
    public static ConfigurationDefinition<string> SaveFolder { get; } = new(nameof(SaveFolder), string.Empty);
    public static ConfigurationDefinition<string> Player { get; } = new(nameof(Player), "FFplay");
    public static ConfigurationDefinition<bool> IsPlayerRect { get; } = new(nameof(IsPlayerRect), false);
    public static ConfigurationDefinition<bool> IsUseProxy { get; } = new(nameof(IsUseProxy), false);
    public static ConfigurationDefinition<string> ProxyUrl { get; } = new(nameof(ProxyUrl), string.Empty);
    public static ConfigurationDefinition<string> CookieChina { get; } = new(nameof(CookieChina), string.Empty);
    public static ConfigurationDefinition<string> CookieOversea { get; } = new(nameof(CookieOversea), string.Empty);
    public static ConfigurationDefinition<string> UserAgent { get; } = new(nameof(UserAgent), string.Empty);
}

[Obfuscation]
public sealed class Room
{
    public string NickName { get; set; } = null!;
    public string RoomUrl { get; set; } = null!;
    public bool IsToNotify { get; set; } = true;
    public bool IsToRecord { get; set; } = true;

    public override string ToString() => $"{RoomUrl},{NickName}";
}
