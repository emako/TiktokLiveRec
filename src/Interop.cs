using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace TiktokLiveRec;

internal static class Interop
{
    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmGetWindowAttribute(nint hwnd, DwmWindowAttribute attr, out int attrValue, int attrSize);

    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmSetWindowAttribute(nint hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);

    public enum DwmWindowAttribute : uint
    {
        NCRenderingEnabled = 1,
        NCRenderingPolicy,
        TransitionsForceDisabled,
        AllowNCPaint,
        CaptionButtonBounds,
        NonClientRtlLayout,
        ForceIconicRepresentation,
        Flip3DPolicy,
        ExtendedFrameBounds,
        HasIconicBitmap,
        DisallowPeek,
        ExcludedFromPeek,
        Cloak,
        Cloaked,
        FreezeRepresentation,
        PassiveUpdateMode,
        UseHostBackdropBrush,
        UseImmersiveDarkMode = 20,
        WindowCornerPreference = 33,
        BorderColor,
        CaptionColor,
        TextColor,
        VisibleFrameBorderThickness,
        SystemBackdropType,
        Last,
    }

    public enum DwmWindowCornerPreference : uint
    {
        DWMWCP_DEFAULT = 0,
        DWMWCP_DONOTROUND = 1,
        DWMWCP_ROUND = 2,
        DWMWCP_ROUNDSMALL = 3
    }

    public static bool IsWindows10Version1809OrAbove()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Version version = Environment.OSVersion.Version;

            if (version.Major == 10 && version.Minor == 0)
            {
                return version.Build >= 17763;
            }
        }

        return false;
    }

    public static nint[] GetWindowHandleByProcessId(int pid)
    {
        List<nint> hWnds = [];

        User32.EnumWindows((hWnd, lParam) =>
        {
            _ = User32.GetWindowThreadProcessId(hWnd, out uint processId);

            if (processId == pid)
            {
                hWnds.Add(hWnd.DangerousGetHandle());
            }
            return true;
        }, IntPtr.Zero);

        return [.. hWnds];
    }

    public static bool IsDarkModeForWindow(nint hWnd)
    {
        if (IsWindows10Version1809OrAbove())
        {
            int hr = DwmGetWindowAttribute(hWnd, DwmWindowAttribute.UseImmersiveDarkMode, out int darkMode, sizeof(int));
            return hr >= 0 && darkMode == 1;
        }
        return true;
    }

    public static bool EnableDarkModeForWindow(nint hWnd, bool enable = true)
    {
        if (IsWindows10Version1809OrAbove())
        {
            int darkMode = enable ? 1 : 0;
            int hr = DwmSetWindowAttribute(hWnd, DwmWindowAttribute.UseImmersiveDarkMode, ref darkMode, sizeof(int));
            return hr >= 0;
        }
        return true;
    }

    public static bool SetRoundedCorners(nint hWnd, bool enable = true)
    {
        if (IsWindows10Version1809OrAbove())
        {
            int preference = enable ? (int)DwmWindowCornerPreference.DWMWCP_ROUND : (int)DwmWindowCornerPreference.DWMWCP_DONOTROUND;
            int hr = DwmSetWindowAttribute(hWnd, DwmWindowAttribute.WindowCornerPreference, ref preference, sizeof(int));
            return hr >= 0;
        }
        return true;
    }

    public static void SetWindowIcon(nint hWnd, Icon icon)
    {
        const uint WM_SETICON = 0x0080;
        const nint ICON_SMALL = 0;
        const nint ICON_BIG = 1;

        int hIcon = (int)icon.Handle;
        _ = User32.SendMessage(hWnd, WM_SETICON, ICON_SMALL, hIcon);
        _ = User32.SendMessage(hWnd, WM_SETICON, ICON_BIG, hIcon);
    }

    public static void SetWindowTitle(nint hWnd, string title)
    {
        _ = User32.SetWindowText(hWnd, title);
    }

    public static void SetHideFromTaskBar(nint hWnd)
    {
        int exStyle = User32.GetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE);

        exStyle &= ~((int)User32.WindowStylesEx.WS_EX_APPWINDOW);
        exStyle |= (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
        _ = User32.SetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE, exStyle);
        _ = User32.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, User32.SetWindowPosFlags.SWP_NOMOVE | User32.SetWindowPosFlags.SWP_NOSIZE | User32.SetWindowPosFlags.SWP_NOZORDER | User32.SetWindowPosFlags.SWP_FRAMECHANGED);
    }
}
