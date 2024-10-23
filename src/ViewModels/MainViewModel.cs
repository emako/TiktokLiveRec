using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using System.IO;
using System.Windows.Controls;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;

namespace TiktokLiveRec;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    public DataGrid RecTable { get; private set; } = null!;

    [RelayCommand]
    private void RecTableLoaded(RelayEventParameter param)
    {
        RecTable = (DataGrid)param.Deconstruct().Sender;
    }

    [RelayCommand]
    private async Task AddRoomAsync()
    {
        AddRoomContentDialog dialog = new();
        ContentDialogResult result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (!string.IsNullOrWhiteSpace(dialog.NickName))
            {
                List<Room> rooms = [.. Configurations.Rooms.Get()];

                rooms.RemoveAll(room => room.LiveUrl == dialog.Url);
                rooms.Add(new Room()
                {
                    NickName = dialog.NickName,
                    LiveUrl = dialog.Url!,
                });
                Configurations.Rooms.Set([.. rooms]);
                ConfigurationManager.Save();
            }
        }
    }

    [RelayCommand]
    private async Task OpenSaveFolderAsync()
    {
        const string path = "download";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(Path.GetFullPath(path)));
    }
}
