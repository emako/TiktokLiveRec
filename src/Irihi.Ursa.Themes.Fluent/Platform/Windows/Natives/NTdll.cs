using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace UrsaAvaloniaUI.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class NTdll
{
    public struct OSVERSIONINFOEX
    {
        public int OSVersionInfoSize;

        public int MajorVersion;

        public int MinorVersion;

        public int BuildNumber;

        public int PlatformId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string CSDVersion;

        public ushort ServicePackMajor;

        public ushort ServicePackMinor;

        public short SuiteMask;

        public byte ProductType;

        public byte Reserved;
    }

    public static class NTStatus
    {
        public const int STATUS_SUCCESS = 0;
    }

    [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [SecurityCritical]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int RtlGetVersion(out OSVERSIONINFOEX versionInfo);

    public static Version GetOSVersion()
    {
        if (RtlGetVersion(out var versionInfo) == 0)
        {
            return new Version(versionInfo.MajorVersion, versionInfo.MinorVersion, versionInfo.BuildNumber, versionInfo.PlatformId);
        }

        return Environment.OSVersion.Version;
    }
}
