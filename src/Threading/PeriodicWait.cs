namespace TiktokLiveRec.Threading;

/// <summary>
/// <seealso cref="PeriodicTimer"/>
/// </summary>
public class PeriodicWait
{
    protected int _interval = 50;

    public TimeSpan Period { get; set; }

    public PeriodicWait(TimeSpan period)
    {
        Period = period;

        if (Period.TotalMilliseconds < _interval)
        {
            _interval = (int)Period.TotalMilliseconds;
        }
    }

    public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
    {
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
