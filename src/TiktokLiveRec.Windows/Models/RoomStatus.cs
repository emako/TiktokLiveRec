namespace TiktokLiveRec.Core;

public sealed class RoomStatus
{
    public string NickName { get; set; } = string.Empty;

    public string RoomUrl { get; set; } = string.Empty;

    public string HlsUrl { get; set; } = string.Empty;

    public StreamStatus StreamStatus { get; set; } = default;

    public RecordStatus RecordStatus => Recorder.RecordStatus;

    public Recorder Recorder { get; } = new();

    public Player Player { get; } = new();
}

public enum StreamStatus
{
    Initialized,
    Disabled,
    NotStreaming,
    Streaming,
}
