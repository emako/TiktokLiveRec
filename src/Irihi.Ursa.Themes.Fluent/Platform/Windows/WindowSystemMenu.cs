using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace UrsaAvaloniaUI.Platform.Windows;

[SupportedOSPlatform("Windows")]
internal class WindowSystemMenu
{
    private const int WM_SYSMENU = 0x313;
    private const int MF_BYCOMMAND = 0x00000000;
    private const int MF_ENABLED = 0x00000000;
    private const int SC_CLOSE = 0xF060;

    [DllImport("user32.dll")]
    private static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

    [DllImport("user32.dll")]
    private static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern bool EnableMenuItem(nint hMenu, uint uIDEnableItem, uint uEnable);

    public static void ShowSystemMenu(Window window, PointerEventArgs e)
    {
        nint hWnd = new WindowInteropHelper(window).Handle;

        nint hMenu = GetSystemMenu(hWnd, false);
        EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_ENABLED);

        var pos = window.PointToScreen(e.GetPosition(window));
        nint lParam = (pos.Y << 16) | (pos.X & 0xFFFF);
        SendMessage(hWnd, WM_SYSMENU, nint.Zero, lParam);
    }
}
