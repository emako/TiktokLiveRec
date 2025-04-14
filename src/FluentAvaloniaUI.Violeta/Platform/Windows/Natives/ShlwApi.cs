using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class ShlwApi
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int PathParseIconLocation([MarshalAs(UnmanagedType.LPWStr)] ref string pszIconFile);
}
