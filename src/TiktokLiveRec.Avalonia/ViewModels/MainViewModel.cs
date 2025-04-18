using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using TiktokLiveRec.Views;

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
    private void AddRoom()
    {
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
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is Window window)
            {
                ContentDialog dialog = new()
                {
                    Title = "About".Tr(),
                    Content = new AboutDialogContent(),
                    CloseButtonText = "ButtonOfClose".Tr(),
                };

                _ = dialog.UseAsTitleBarForWindowFrame(window, true)
                    .UseAsTitleBarForWindowSystemMenu(window);
                _ = await dialog.ShowAsync();
            }
        }
    }
}
