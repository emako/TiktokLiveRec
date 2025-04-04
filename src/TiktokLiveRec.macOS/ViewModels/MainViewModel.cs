using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace TiktokLiveRec.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<RoomStatusReactive> roomStatuses;

    public MainViewModel()
    {
        RoomStatuses = new ObservableCollection<RoomStatusReactive>()
        {
            new RoomStatusReactive()
            {
                NickName = "123",
                RoomUrl = "123",
            },
            new RoomStatusReactive()
            {
                NickName = "123",
                RoomUrl = "123",
            },
            new RoomStatusReactive()
            {
                NickName = "123",
                RoomUrl = "123",
            },
        };
    }
}
