using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace UrsaAvaloniaUI.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class UxTheme
{
    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "#132", SetLastError = true)]
    public static extern bool ShouldAppsUseDarkMode();

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "#135", SetLastError = true)]
    public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "#135", SetLastError = true)]
    [Obsolete("Since the support for AllowDarkModeForApp is uncertain, it will not be considered for use.")]
    public static extern void AllowDarkModeForApp(bool allowDark);

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "#136", SetLastError = true)]
    public static extern void FlushMenuThemes();

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "#138", SetLastError = true)]
    public static extern bool ShouldSystemUseDarkMode();

    public enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    }
}
