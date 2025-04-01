using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using UrsaAvaloniaUI.Platform.Windows.Natives;

namespace UrsaAvaloniaUI.Platform.Windows;

/// <summary>
/// https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/apply-snap-layout-menu
/// </summary>
[SupportedOSPlatform("Windows")]
public static class SnapLayout
{
    public static bool IsSupported => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build > 20000;
    public static bool IsEnabled { get; } = IsSnapLayoutEnabled();

    public static void EnableWindowsSnapLayout(Window window, Button maximize)
    {
        if (window == null || maximize == null) return;

        bool pointerOnMaxButton = false;
        PropertyInfo? isPointerOverSetter = typeof(Button).GetProperty(nameof(Button.IsPointerOver));
        nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam, ref bool handled)
        {
            switch ((User32.WindowMessage)msg)
            {
                case User32.WindowMessage.WM_CAPTURECHANGED:
                    if (!pointerOnMaxButton) break;
                    window.WindowState = window.WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
                    break;

                case User32.WindowMessage.WM_NCHITTEST:
                    var point = new PixelPoint(
                        (short)(ToInt32(lParam) & 0xffff),
                        (short)(ToInt32(lParam) >> 16));
                    var desiredSize = maximize.DesiredSize;
                    var buttonLeftTop = maximize.PointToScreen(window.FlowDirection == FlowDirection.LeftToRight
                        ? new Point(desiredSize.Width, 0)
                        : new Point(0, 0));
                    var x = (buttonLeftTop.X - point.X) / window.RenderScaling;
                    var y = (point.Y - buttonLeftTop.Y) / window.RenderScaling;
                    if (new Rect(0, 0,
                            desiredSize.Width,
                            desiredSize.Height)
                        .Contains(new Point(x, y)))
                    {
                        isPointerOverSetter?.SetValue(maximize, true);
                        pointerOnMaxButton = true;
                        handled = true;
                        return (nint)User32.HitTestValues.HTMAXBUTTON;
                    }

                    pointerOnMaxButton = false;
                    isPointerOverSetter?.SetValue(maximize, false);
                    break;
            }

            return IntPtr.Zero;

            static int ToInt32(nint ptr)
            {
                return IntPtr.Size == 4
                    ? ptr.ToInt32()
                    : (int)(ptr.ToInt64() & 0xffffffff);
            }
        }

        window.SizeChanged += (_, _) => isPointerOverSetter?.SetValue(maximize, false);

        Win32Properties.AddWndProcHookCallback(window, new Win32Properties.CustomWndProcHookCallback(WndProc));
    }

    private static bool IsSnapLayoutEnabled()
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
            object? registryValueObject = key?.GetValue("EnableSnapAssistFlyout");

            if (registryValueObject == null)
            {
                return true;
            }
            int registryValue = (int)registryValueObject;
            return registryValue > 0;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        return true;
    }
}
