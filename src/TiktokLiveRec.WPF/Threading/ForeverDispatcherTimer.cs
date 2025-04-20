using System.Windows.Threading;

namespace TiktokLiveRec.Threading;

public class ForeverDispatcherTimer(TimeSpan interval, Action callback, DispatcherPriority priority = DispatcherPriority.Normal, Dispatcher? dispatcher = null)
    : DispatcherTimer(interval, priority, (_, _) => callback.Invoke(), dispatcher ?? Dispatcher.CurrentDispatcher);
