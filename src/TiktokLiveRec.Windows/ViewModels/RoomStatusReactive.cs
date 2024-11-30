using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using System.IO;
using TiktokLiveRec.Core;
using TiktokLiveRec.Models;
using Windows.System;
using Wpf.Ui.Violeta.Controls;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class RoomStatusReactive : ReactiveObject
{
    [ObservableProperty]
    private string nickName = string.Empty;

    [ObservableProperty]
    private string avatarThumbUrl = string.Empty;

    [ObservableProperty]
    private string roomUrl = string.Empty;

    [ObservableProperty]
    private string flvUrl = string.Empty;

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
        RecordStatus.Recording => "RecordStatusOfRecording".Tr() + " " + Duration,
#pragma warning disable CS0618 // Type or member is obsolete
        RecordStatus.Error => "RecordStatusOfError".Tr(),
#pragma warning restore CS0618 // Type or member is obsolete
        _ => "RecordStatusOfUnknown".Tr(),
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    public DateTime startTime = DateTime.MinValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    public DateTime endTime = DateTime.MinValue;

    public string Duration
    {
        get
        {
            if (StartTime != DateTime.MinValue)
            {
                if (EndTime != DateTime.MinValue)
                {
                    return (EndTime - StartTime).ToTimeCodeString();
                }
                return (DateTime.Now - StartTime).ToTimeCodeString();
            }
            return string.Empty;
        }
    }

    public void RefreshStatus()
    {
        OnPropertyChanged(nameof(StreamStatusText));
        OnPropertyChanged(nameof(RecordStatusText));
    }

    public void RefreshDuration()
    {
        if (RecordStatus == RecordStatus.Recording)
        {
            OnPropertyChanged(nameof(RecordStatusText));
            OnPropertyChanged(nameof(Duration));
        }
    }

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
        // TODO: Implement for other platforms
        await Launcher.LaunchUriAsync(new Uri(RoomUrl));
    }
}

public sealed class CommandEventArgs(string command) : EventArgs
{
    public string Command { get; } = command;
}

file static class TimeSpanExtension
{
    public static string ToTimeCodeString(this TimeSpan timeSpan)
    {
        timeSpan = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        if (timeSpan.TotalHours < 1)
        {
            return timeSpan.ToString(@"mm\:ss");
        }
        else
        {
            return timeSpan.ToString(@"h\:mm\:ss");
        }
    }
}
