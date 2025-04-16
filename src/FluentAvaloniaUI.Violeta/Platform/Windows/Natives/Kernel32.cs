using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class Kernel32
{
    [DllImport("kernel32.dll", SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
    public static extern nint LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true)]
    public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_USER_PRESENT = 0x00000004,
    }
}
