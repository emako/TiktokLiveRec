using TiktokLiveRec.Core;

namespace TiktokLiveRec.Models;

public sealed class RoomStatus
{
    public string NickName { get; set; } = string.Empty;

    public string AvatarThumbUrl { get; set; } = string.Empty;

    public string RoomUrl { get; set; } = string.Empty;

    public string FlvUrl { get; set; } = string.Empty;

    public string HlsUrl { get; set; } = string.Empty;

    public StreamStatus StreamStatus { get; set; } = default;

    public RecordStatus RecordStatus
    {
        get => Recorder.RecordStatus;
        internal set => Recorder.RecordStatus = value;
    }

    public Recorder Recorder { get; } = new();
}

public enum StreamStatus
{
    Initialized,
    Disabled,
    NotStreaming,
    Streaming,
}
