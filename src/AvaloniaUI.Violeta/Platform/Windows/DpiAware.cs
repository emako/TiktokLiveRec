using System.Runtime.Versioning;
using AvaloniaUI.Violeta.Platform.Windows.Natives;

namespace AvaloniaUI.Violeta.Platform.Windows;

[SupportedOSPlatform("Windows")]
public static class DpiAware
{
    public static bool SetProcessDpiAwareness(int awareness = 2)
    {
        if (NTdll.RtlGetVersion(out var versionInfo) == 0)
        {
            Version version = new(versionInfo.MajorVersion, versionInfo.MinorVersion, versionInfo.BuildNumber, versionInfo.PlatformId);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && version >= new Version(6, 3) && SHCore.SetProcessDpiAwareness((SHCore.PROCESS_DPI_AWARENESS)awareness) == 0)
            {
                return true;
            }
        }

        return false;
    }
}
