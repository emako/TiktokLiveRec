using System.Windows.Threading;

namespace TiktokLiveRec.Threading;

public class ForeverDispatcherTimer : DispatcherTimer
{
    public ForeverDispatcherTimer(TimeSpan interval, Action callback, DispatcherPriority priority = DispatcherPriority.Normal, Dispatcher? dispatcher = null)
        : base(interval, priority, (_, _) => callback.Invoke(), dispatcher ?? Dispatcher.CurrentDispatcher)
    {
    }
}
