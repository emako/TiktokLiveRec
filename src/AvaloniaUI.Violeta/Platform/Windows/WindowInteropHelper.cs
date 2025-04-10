using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace AvaloniaUI.Violeta.Platform.Windows;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

/// <summary>
/// Implements Avalon WindowInteropHelper classes, which helps
/// interop b/w legacy and Avalon Window.
/// Ported from https://github.com/dotnet/wpf/blob/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Interop/WindowInteropHelper.cs
/// </summary>
[SupportedOSPlatform("Windows")]
public sealed class WindowInteropHelper
{
    private readonly Window _window;

    /// <summary>
    /// Get the Handle of the window
    /// </summary>
    /// <remarks>
    ///     Callers must have UIPermission(UIPermissionWindow.AllWindows) to call this API.
    /// </remarks>
    public nint Handle { get; } = IntPtr.Zero;

    /// <summary>
    /// Get/Set the Owner handle of the window
    /// </summary>
    /// <remarks>
    ///     Callers must have UIPermission(UIPermissionWindow.AllWindows) to call this API.
    /// </remarks>
    public nint Owner
    {
        get
        {
            Debug.Assert(_window != null, "Cannot be null since we verify in the constructor");
            return new WindowInteropHelper(_window.Owner!.PlatformImpl).Handle;
        }
    }

    public WindowInteropHelper(Window window) : this(window.PlatformImpl)
    {
        _window = window;
    }

    private WindowInteropHelper(IWindowBaseImpl? platformImpl)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (platformImpl!.GetType().GetProperty("Hwnd", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(platformImpl) is nint handle)
            {
                Handle = handle;
            }
        }
    }

    public WindowInteropHelper(TopLevel topLevel)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (topLevel is Window window)
            {
                _window = window;

                IWindowBaseImpl? platformImpl = window.PlatformImpl;

                if (platformImpl!.GetType().GetProperty("Hwnd", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(platformImpl) is nint handle)
                {
                    Handle = handle;
                }
            }
            else
            {
                if (topLevel.PlatformImpl is IPopupImpl popupImpl)
                {
                    if (popupImpl.GetType().GetProperty("Hwnd", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(popupImpl) is nint handle)
                    {
                        Handle = handle;
                    }
                }
            }
        }
    }

    public static nint GetHwndForControl(Control control)
    {
        if (control is TopLevel)
        {
            return new WindowInteropHelper((control as TopLevel)!).Handle;
        }
        else if (control is Visual visual && visual.GetVisualRoot() is TopLevel topLevel)
        {
            return new WindowInteropHelper(topLevel).Handle;
        }
        return IntPtr.Zero;
    }
}
