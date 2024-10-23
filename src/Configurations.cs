using Fischless.Configuration;
using System.Reflection;

namespace TiktokLiveRec;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<Room[]> Rooms { get; } = new(nameof(Room), []);
}

public class Room
{
    public string NickName { get; set; } = null!;
    public string LiveUrl { get; set; } = null!;

    public override string ToString() => $"{LiveUrl},{NickName}";
}
