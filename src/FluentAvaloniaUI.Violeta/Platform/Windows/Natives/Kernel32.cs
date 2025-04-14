using System;
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
}
