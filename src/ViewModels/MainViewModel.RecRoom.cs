using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;

namespace TiktokLiveRec.ViewModels;

public partial class MainViewModel
{
    [ObservableObject]
    public partial class RecRoom : ReactiveObject
    {
        [ObservableProperty]
        private string nickName = string.Empty;

        [ObservableProperty]
        private string roomUrl = string.Empty;

        [ObservableProperty]
        private string m3u8Url = string.Empty;

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
            await Task.CompletedTask;
        }
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
}
