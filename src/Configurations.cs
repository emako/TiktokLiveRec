using Fischless.Configuration;
using System.Reflection;

namespace TiktokLiveRec;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<string> Theme { get; } = new(nameof(Theme), string.Empty);
    public static ConfigurationDefinition<Room[]> Rooms { get; } = new(nameof(Rooms), []);
}

public class Room
{
    public string NickName { get; set; } = null!;
    public string LiveUrl { get; set; } = null!;

    public override string ToString() => $"{LiveUrl},{NickName}";
}
