using CommunityToolkit.Mvvm.ComponentModel;

namespace TiktokLiveRec.ViewModels;

public partial class RoomStatusReactive : ObservableObject
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
}
