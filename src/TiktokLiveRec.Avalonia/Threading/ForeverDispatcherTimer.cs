using Avalonia.Threading;

namespace TiktokLiveRec.Threading;

public class ForeverDispatcherTimer(TimeSpan interval, Action callback)
    : DispatcherTimer(interval, DispatcherPriority.ApplicationIdle, (_, _) => callback.Invoke());
