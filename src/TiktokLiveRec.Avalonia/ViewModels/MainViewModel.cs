using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fischless.Configuration;
using FluentAvalonia.UI.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using TiktokLiveRec.Views;
using Ursa.Controls;

namespace TiktokLiveRec.ViewModels;

public sealed partial class MainViewModel : ObservableObject
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
    private async Task AddRoomAsync()
    {
        AddRoomContentDialogContent content = new();
        _ = await new ContentDialog()
        {
            Content = content,
        }.ShowAsync();

        if (content.Result == ContentDialogResult.Primary)
        {
            if (!string.IsNullOrWhiteSpace(content.NickName))
            {
                List<Room> rooms = [.. Configurations.Rooms.Get()];

                rooms.RemoveAll(room => room.RoomUrl == content.Url);
                rooms.Add(new Room()
                {
                    NickName = content.NickName,
                    RoomUrl = content.RoomUrl!,
                });
                Configurations.Rooms.Set([.. rooms]);
                ConfigurationManager.Save();

                RoomStatuses.Add(new RoomStatusReactive()
                {
                    NickName = content.NickName,
                    RoomUrl = content.RoomUrl!,
                });
            }
        }
    }

    [RelayCommand]
    private void OpenSettingsDialog()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            foreach (SettingsWindow win in desktop.Windows.OfType<SettingsWindow>().ToArray())
            {
                win.Close();
            }

            if (desktop.MainWindow?.IsVisible ?? false)
            {
                SettingsWindow win = new()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };
                win.ShowDialog(desktop.MainWindow);
            }
            else
            {
                SettingsWindow win = new()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                win.Show();
            }
        }
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
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    private async Task OpenAboutAsync()
    {
        _ = await new ContentDialog()
        {
            Content = new AboutDialogContent(),
        }.ShowAsync();
    }
}
