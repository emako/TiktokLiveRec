namespace TiktokLiveRec.Core;

public class RoomStatus
{
    public string NickName { get; set; } = string.Empty;

    public string RoomUrl { get; set; } = string.Empty;

    public string HlsUrl { get; set; } = string.Empty;

    public StreamStatus StreamStatus { get; set; } = default;

    public RecordStatus RecordStatus { get; set; } = default;
}

public enum StreamStatus
{
    Initialized,
    Disabled,
    NotStreaming,
    Streaming,
}

public enum RecordStatus
{
    Initialized,
    Disabled,
    NotRecording,
    Recording,
}
