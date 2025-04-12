using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TiktokLiveRec.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<RoomStatusReactive> roomStatuses;

    public MainViewModel()
    {
        RoomStatuses =
        [
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
            }

        ];
    }

    [RelayCommand]
    private void OpenSaveFolder()
    {
    }

    [RelayCommand]
    private void OpenSettingsFileFolder()
    {
    }

    [RelayCommand]
    private void OpenAbout()
    {
    }
}
