using Avalonia.Platform;

namespace AvaloniaUI.Violeta.Resources;

public static class ResourceLoader
{
    public static Stream GetStream(string uriString)
    {
        Stream stream = AssetLoader.Open(new Uri(uriString));
        return stream;
    }

    public static string? GetText(string uriString)
    {
        using Stream stream = AssetLoader.Open(new Uri(uriString));

        if (stream == null)
            return null;

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
