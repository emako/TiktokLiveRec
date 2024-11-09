using System.Diagnostics;

namespace TiktokLiveRec.Core;

public sealed class Recorder
{
    public RecordStatus RecordStatus { get; private set; } = RecordStatus.Initialized;

    public Process? RecorderProcess { get; private set; } = null;

    public void Start()
    {
        if (RecordStatus == RecordStatus.Recording)
        {
            // Already recording
            return;
        }

        // TODO
    }

    public void Stop()
    {
        // TODO
    }
}

public enum RecordStatus
{
    Initialized,
    Disabled,
    NotRecording,
    Recording,
}
