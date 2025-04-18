using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Taskbar;

[SupportedOSPlatform("Windows")]
public enum ThumbnailAlphaType
{
    Unknown = 0,

    NoAlphaChannel = 1,

    HasAlphaChannel = 2,
}

[SupportedOSPlatform("Windows")]
internal enum KnownDestinationCategory
{
    Frequent = 1,
    Recent
}

[SupportedOSPlatform("Windows")]
internal enum SetTabPropertiesOption
{
    None = 0x0,
    UseAppThumbnailAlways = 0x1,
    UseAppThumbnailWhenActive = 0x2,
    UseAppPeekAlways = 0x4,
    UseAppPeekWhenActive = 0x8
}

[SupportedOSPlatform("Windows")]
internal enum TaskbarProgressBarStatus
{
    NoProgress = 0,
    Indeterminate = 0x1,
    Normal = 0x2,
    Error = 0x4,
    Paused = 0x8
}

[SupportedOSPlatform("Windows")]
internal enum ThumbButtonMask
{
    Bitmap = 0x1,
    Icon = 0x2,
    Tooltip = 0x4,
    THB_FLAGS = 0x8
}

[SupportedOSPlatform("Windows")]
[Flags]
internal enum ThumbButtonOptions
{
    Enabled = 0x00000000,
    Disabled = 0x00000001,
    DismissOnClick = 0x00000002,
    NoBackground = 0x00000004,
    Hidden = 0x00000008,
    NonInteractive = 0x00000010
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct ThumbButton
{
    internal const int Clicked = 0x1800;

    [MarshalAs(UnmanagedType.U4)]
    internal ThumbButtonMask Mask;

    internal uint Id;
    internal uint Bitmap;
    internal nint Icon;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    internal string Tip;

    [MarshalAs(UnmanagedType.U4)]
    internal ThumbButtonOptions Flags;
}
