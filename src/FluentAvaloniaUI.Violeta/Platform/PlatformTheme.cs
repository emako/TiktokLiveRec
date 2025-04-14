using FluentAvalonia.UI.Violeta.Platform.macOS;
using FluentAvalonia.UI.Violeta.Platform.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform;

[SupportedOSPlatform("Windows")]
[SupportedOSPlatform("macOS")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public static class PlatformTheme
{
    public static bool AppsUseDarkTheme()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return WindowsTheme.AppsUseDarkTheme();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // Use system instead
            return OSXTheme.SystemUsesDarkTheme();
        }

        return true;
    }

    public static bool SystemUsesDarkTheme()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return WindowsTheme.SystemUsesDarkTheme();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSXTheme.SystemUsesDarkTheme();
        }

        return true;
    }
}
