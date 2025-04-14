using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public static class WindowsTheme
{
    public static bool AppsUseDarkTheme()
    {
        var value = Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "AppsUseLightTheme", 1);

        return value != null && (int)value == 0;
    }

    public static bool SystemUsesDarkTheme()
    {
        var value = Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "SystemUsesLightTheme", 0);

        return value == null || (int)value == 0;
    }
}
