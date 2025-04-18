using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TiktokLiveRec.Extensions;

internal static class UrlHelper
{
    public static void OpenUrl(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Failed to open URL: {e}");
        }
    }
}
