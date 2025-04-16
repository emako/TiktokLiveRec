using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
internal static class DefaultIconSize
{
    public static readonly SIZE ExtraLarge = new(256, 256);
    public static readonly SIZE Large = new(48, 48);
    public static readonly SIZE Maximum = new(256, 256);
    public static readonly SIZE Medium = new(32, 32);
    public static readonly SIZE Small = new(16, 16);
}

[SupportedOSPlatform("Windows")]
internal static class DefaultThumbnailSize
{
    public static readonly SIZE ExtraLarge = new(1024, 1024);
    public static readonly SIZE Large = new(256, 256);
    public static readonly SIZE Maximum = new(1024, 1024);
    public static readonly SIZE Medium = new(96, 96);
    public static readonly SIZE Small = new(32, 32);
}
