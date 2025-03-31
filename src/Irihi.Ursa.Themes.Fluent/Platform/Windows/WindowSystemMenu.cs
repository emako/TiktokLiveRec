﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Runtime.Versioning;
using UrsaAvaloniaUI.Platform.Windows.Natives;

namespace UrsaAvaloniaUI.Platform.Windows;

[SupportedOSPlatform("Windows")]
public static class WindowSystemMenu
{
    [Obsolete("Isn't suitable for Avalonia")]
    public static int GetTitleBarHeight(nint hwnd)
    {
        if (Dwmapi.DwmGetWindowAttribute(hwnd, Dwmapi.DWMWA_CAPTION_HEIGHT, out int height, sizeof(int)) == 0)
        {
            return height;
        }

        // Fallback to -1.
        return -1;
    }

    public static void ShowSystemMenu(Window window, PointerEventArgs e)
    {
        nint hWnd = new WindowInteropHelper(window).Handle;

        nint hMenu = User32.GetSystemMenu(hWnd, false);
        User32.EnableMenuItem(hMenu, User32.SC_CLOSE, User32.MF_BYCOMMAND | User32.MF_ENABLED);

        var pos = window.PointToScreen(e.GetPosition(window));
        nint lParam = (pos.Y << 16) | (pos.X & 0xFFFF);
        User32.SendMessage(hWnd, User32.WM_SYSMENU, nint.Zero, lParam);
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
