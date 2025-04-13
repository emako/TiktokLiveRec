using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace TiktokLiveRec;

internal static class Interop
{
    [SupportedOSPlatform("Windows")]
    private static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        public static extern int GetUserDefaultLocaleName(StringBuilder lpLocaleName, int cchLocaleName);
    }

    [SupportedOSPlatform("Windows")]
    public static string GetUserDefaultLocaleName()
    {
        StringBuilder localeName = new(85);
        int result = Kernel32.GetUserDefaultLocaleName(localeName, localeName.Capacity);
        return result > 0 ? localeName.ToString() : string.Empty;
    }
}
