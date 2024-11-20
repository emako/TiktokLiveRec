namespace TiktokLiveRec.Threading;

public class PeriodicWait
{
    protected int _interval = 50;
    protected bool _initialed = false;

    public TimeSpan InitialDelay { get; set; }

    public TimeSpan Period { get; set; }

    public PeriodicWait(TimeSpan period, TimeSpan initialDelay = default)
    {
        InitialDelay = initialDelay;
        Period = period;

        if (Period.TotalMilliseconds < _interval)
        {
            _interval = (int)Period.TotalMilliseconds;
        }
    }

    public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
    {
        if (!_initialed)
        {
            _initialed = true;
            if (InitialDelay >= TimeSpan.Zero)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return ValueTask.FromResult(false);
                }

                Thread.Sleep(InitialDelay);
                return ValueTask.FromResult(true);
            }
        }

        int currentInterval = default;
        while (currentInterval < Period.TotalMilliseconds)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromResult(false);
            }

            Thread.Sleep(_interval);
            currentInterval += _interval;
        }

        return ValueTask.FromResult(true);
    }
}
