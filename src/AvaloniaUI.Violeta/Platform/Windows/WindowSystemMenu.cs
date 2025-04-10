using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Runtime.Versioning;
using AvaloniaUI.Violeta.Platform.Windows.Natives;

namespace AvaloniaUI.Violeta.Platform.Windows;

[SupportedOSPlatform("Windows")]
public static class WindowSystemMenu
{
    public static void ShowSystemMenu(Window window, PointerEventArgs e)
    {
        nint hWnd = new WindowInteropHelper(window).Handle;

        nint hMenu = User32.GetSystemMenu(hWnd, false);
        _ = User32.EnableMenuItem(hMenu, (uint)User32.SysCommand.SC_CLOSE, (uint)(User32.MenuFlags.MF_BYCOMMAND | User32.MenuFlags.MF_ENABLED));

        var pos = window.PointToScreen(e.GetPosition(window));
        nint lParam = (pos.Y << 16) | (pos.X & 0xFFFF);
        _ = User32.SendMessage(hWnd, (int)User32.WindowMessage.WM_SYSMENU, nint.Zero, lParam);
    }

    public static void ApplySystemMenuTheme(bool isDark)
    {
        if (Environment.OSVersion.Version.Build < 18362)
        {
            return;
        }

        if (isDark)
        {
            _ = UxTheme.SetPreferredAppMode(UxTheme.PreferredAppMode.ForceDark);
        }
        else
        {
            _ = UxTheme.SetPreferredAppMode(UxTheme.PreferredAppMode.ForceLight);
        }
        UxTheme.FlushMenuThemes();
    }
}
