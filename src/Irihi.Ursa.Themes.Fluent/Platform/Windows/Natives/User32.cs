using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace UrsaAvaloniaUI.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class User32
{
    public const int WM_SYSMENU = 0x313;
    public const int MF_BYCOMMAND = 0x00000000;
    public const int MF_ENABLED = 0x00000000;
    public const int SC_CLOSE = 0xF060;

    [DllImport("user32.dll")]
    public static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

    [DllImport("user32.dll")]
    public static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32.dll")]
    public static extern bool EnableMenuItem(nint hMenu, uint uIDEnableItem, uint uEnable);
}
