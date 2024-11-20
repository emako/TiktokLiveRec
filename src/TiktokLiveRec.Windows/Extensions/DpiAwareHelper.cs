using Vanara.PInvoke;

namespace TiktokLiveRec.Extensions;

public static class DpiAwareHelper
{
    public static bool SetProcessDpiAwareness()
    {
        if (SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE) == 0)
        {
            return true;
        }
        return false;
    }
}
