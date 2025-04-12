using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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
        }, nint.Zero);

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
        _ = User32.SetWindowPos(hWnd, nint.Zero, 0, 0, 0, 0, User32.SetWindowPosFlags.SWP_NOMOVE | User32.SetWindowPosFlags.SWP_NOSIZE | User32.SetWindowPosFlags.SWP_NOZORDER | User32.SetWindowPosFlags.SWP_FRAMECHANGED);
    }

    /// <param name="ratio">Screen ratio in maximum orientation</param>
    public static void SetWindowCenterRatio(nint hWnd, double ratio = 1d)
    {
        // Fit the window size and center it.
        if (User32.GetWindowRect(hWnd, out RECT lpRect))
        {
            Screen screen = Screen.FromHandle(hWnd);
            int screenWidth = screen.WorkingArea.Width;
            int screenHeight = screen.WorkingArea.Height;

            // Calculate the current width and height of the window
            int windowWidth = lpRect.Right - lpRect.Left;
            int windowHeight = lpRect.Bottom - lpRect.Top;

            // Define the maximum allowed width and height (80% of the screen size)
            int maxWidth = (int)(screenWidth * ratio);
            int maxHeight = (int)(screenHeight * ratio);

            // Check if the window exceeds the screen's 80% size and scale down if necessary
            if (windowWidth > maxWidth || windowHeight > maxHeight)
            {
                // Calculate the scaling factor
                float scaleWidth = (float)maxWidth / windowWidth;
                float scaleHeight = (float)maxHeight / windowHeight;

                // Use the smaller scaling factor to maintain the aspect ratio
                float scaleFactor = Math.Min(scaleWidth, scaleHeight);

                // Calculate the new size
                int newWidth = (int)(windowWidth * scaleFactor);
                int newHeight = (int)(windowHeight * scaleFactor);

                // Calculate the new position to center the window on the screen
                int newX = (screenWidth - newWidth) / 2;
                int newY = (screenHeight - newHeight) / 2;

                // Set the new window size and position (you can use SetWindowPos or a similar API)
                User32.SetWindowPos(hWnd, nint.Zero, newX, newY, newWidth, newHeight, User32.SetWindowPosFlags.SWP_NOZORDER);
            }
            else
            {
                // If the window doesn't exceed 80% of the screen, just center it
                int newX = (screenWidth - windowWidth) / 2;
                int newY = (screenHeight - windowHeight) / 2;

                // Move the window to the centered position without resizing
                User32.SetWindowPos(hWnd, nint.Zero, newX, newY, windowWidth, windowHeight, User32.SetWindowPosFlags.SWP_NOZORDER);
            }
        }
    }

    public static void RestoreWindow(nint hWnd)
    {
        if (User32.IsWindow(hWnd))
        {
            _ = User32.SendMessage(hWnd, User32.WindowMessage.WM_SYSCOMMAND, User32.SysCommand.SC_RESTORE, 0);
            _ = User32.SetForegroundWindow(hWnd);

            if (User32.IsIconic(hWnd))
            {
                _ = User32.ShowWindow(hWnd, ShowWindowCommand.SW_RESTORE);
            }

            _ = User32.BringWindowToTop(hWnd);
            _ = User32.SetActiveWindow(hWnd);
        }
    }

    public static void Attach(uint pid)
    {
        if (Kernel32.AttachConsole(pid))
        {
            Console.WriteLine("Successfully attached to the console of the specified process.");
            Console.WriteLine("Hello from the attached console!");
        }
        else
        {
            Console.WriteLine("Failed to attach to the console of the specified process.");
            Console.WriteLine($"Error Code: {Kernel32.GetLastError()}");
        }
    }

    public static unsafe int? GetParentProcessId(int pid)
    {
        using var hProcess = Kernel32.OpenProcess(ACCESS_MASK.GENERIC_READ, false, (uint)pid);

        if (hProcess == nint.Zero)
        {
            return null!;
        }

        NtDll.PROCESS_BASIC_INFORMATION pbi = new();
        NTStatus status = NtDll.NtQueryInformationProcess(hProcess, NtDll.PROCESSINFOCLASS.ProcessBasicInformation, (nint)(&pbi), (uint)Marshal.SizeOf<NtDll.PROCESS_BASIC_INFORMATION>(), out var returnLength);

        if (status == NTStatus.STATUS_SUCCESS)
        {
            return (int)pbi.InheritedFromUniqueProcessId;
        }
        else
        {
            return null!;
        }
    }

    public static int[] GetChildProcessId(int pid)
    {
        return Process.GetProcesses()
            .Where(p => GetParentProcessId(p.Id) == pid)
            .Select(p => p.Id)
            .ToArray();
    }

    public static (int, string)[] GetChildProcessIdAndName(int pid)
    {
        return Process.GetProcesses()
            .Where(p => GetParentProcessId(p.Id) == pid)
            .Select(p => (p.Id, p.ProcessName))
            .ToArray();
    }

    public static string GetUserDefaultLocaleName()
    {
        StringBuilder localeName = new(85);
        int result = Kernel32.GetUserDefaultLocaleName(localeName, localeName.Capacity);
        return result > 0 ? localeName.ToString() : string.Empty;
    }

    public static bool ExitWindowsEx(User32.ExitWindowsFlags uFlags)
    {
        HPROCESS hProc = Kernel32.GetCurrentProcess();
        AdvApi32.OpenProcessToken(hProc, AdvApi32.TokenAccess.TOKEN_ADJUST_PRIVILEGES | AdvApi32.TokenAccess.TOKEN_QUERY, out AdvApi32.SafeHTOKEN hToken);
        AdvApi32.LookupPrivilegeValue(null, "SeShutdownPrivilege", out AdvApi32.LUID luid);
        AdvApi32.AdjustTokenPrivileges(hToken, false, new AdvApi32.TOKEN_PRIVILEGES(luid, AdvApi32.PrivilegeAttributes.SE_PRIVILEGE_ENABLED), out _);
        return User32.ExitWindowsEx(uFlags, SystemShutDownReason.SHTDN_REASON_MAJOR_NONE);
    }
}
