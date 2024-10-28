using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using TiktokLiveRec.Core;
using Windows.System;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class RoomStatusReactive : ReactiveObject
{
    [ObservableProperty]
    private string nickName = string.Empty;

    [ObservableProperty]
    private string roomUrl = string.Empty;

    [ObservableProperty]
    private string hlsUrl = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StreamStatusText))]
    private StreamStatus streamStatus = default;

    public string StreamStatusText => StreamStatus switch
    {
        StreamStatus.Initialized => "初始化",
        StreamStatus.Disabled => "已禁用",
        StreamStatus.NotStreaming => "未开播",
        StreamStatus.Streaming => "直播中",
        _ => "未知",
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordStatusText))]
    private RecordStatus recordStatus = default;

    public string RecordStatusText => RecordStatus switch
    {
        RecordStatus.Initialized => "初始化",
        RecordStatus.Disabled => "已禁用",
        RecordStatus.NotRecording => "未录制",
        RecordStatus.Recording => "录制中",
        _ => "未知",
    };

    [RelayCommand]
    private async Task GotoRoomUrlAsync()
    {
        await Launcher.LaunchUriAsync(new Uri(RoomUrl));
    }
}
