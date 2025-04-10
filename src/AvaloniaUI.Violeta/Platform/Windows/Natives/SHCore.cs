using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace AvaloniaUI.Violeta.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class SHCore
{
    public enum PROCESS_DPI_AWARENESS
    {
        PROCESS_DPI_UNAWARE,
        PROCESS_SYSTEM_DPI_AWARE,
        PROCESS_PER_MONITOR_DPI_AWARE
    }

    public enum MONITOR_DPI_TYPE
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
        MDT_DEFAULT = 0
    }

    [DllImport("shcore.dll")]
    public static extern uint SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

    [DllImport("shcore.dll")]
    public static extern int GetDpiForMonitor(nint hMonitor, MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY);
}
