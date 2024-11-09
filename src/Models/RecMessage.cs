namespace TiktokLiveRec.Models;

public sealed class RecMessage
{
    public RecMessageType Type { get; set; } = default;
    public object? Data { get; set; } = null;
}

public enum RecMessageType
{
    Unknown,
    Default,
    StartStreaming, // ?
    StopStreaming, // ?
    StartRecording, // ?
    StopRecording, // ?
}
