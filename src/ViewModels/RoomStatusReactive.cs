using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using System.IO;
using TiktokLiveRec.Core;
using Windows.System;
using Wpf.Ui.Violeta.Controls;

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
    private bool isToNotify = true;

    [ObservableProperty]
    private bool isToRecord = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StreamStatusText))]
    private StreamStatus streamStatus = default;

    public string StreamStatusText => StreamStatus switch
    {
        StreamStatus.Initialized => "StreamStatusOfInitialized".Tr(),
        StreamStatus.Disabled => "StreamStatusOfDisabled".Tr(),
        StreamStatus.NotStreaming => "StreamStatusOfNotStreaming".Tr(),
        StreamStatus.Streaming => "StreamStatusOfStreaming".Tr(),
        _ => "StreamStatusOfUnknown".Tr(),
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordStatusText))]
    private RecordStatus recordStatus = default;

    public string RecordStatusText => RecordStatus switch
    {
        RecordStatus.Initialized => "RecordStatusOfInitialized".Tr(),
        RecordStatus.Disabled => "RecordStatusOfDisabled".Tr(),
        RecordStatus.NotRecording => "RecordStatusOfNotRecording".Tr(),
        RecordStatus.Recording => "RecordStatusOfRecording".Tr(),
        RecordStatus.Error => "RecordStatusOfError".Tr(),
        _ => "RecordStatusOfUnknown".Tr(),
    };

    [RelayCommand]
    private async Task PlayRecordAsync()
    {
        if (GlobalMonitor.RoomStatus.TryGetValue(RoomUrl, out RoomStatus? roomStatus)
         && File.Exists(roomStatus.Recorder.FileName))
        {
            await Player.PlayAsync(roomStatus.Recorder.FileName, isSeekable: roomStatus.RecordStatus == RecordStatus.Recording);
        }
        else
        {
            Toast.Warning("PlayerErrorOfNoFile".Tr());
        }
    }

    [RelayCommand]
    private async Task GotoRoomUrlAsync()
    {
        await Launcher.LaunchUriAsync(new Uri(RoomUrl));
    }
}

public sealed class CommandEventArgs(string command) : EventArgs
{
    public string Command { get; } = command;
}
