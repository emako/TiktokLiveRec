using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;
using DataGrid = System.Windows.Controls.DataGrid;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    public DispatcherTimer DispatcherTimer = new()
    {
        Interval = TimeSpan.FromSeconds(3)
    };

    [ObservableProperty]
    private ReactiveCollection<RoomStatusReactive> roomStatuses = [];

    [ObservableProperty]
    private RoomStatusReactive selectedItem = new();

    public MainViewModel()
    {
        RoomStatuses.Reset(Configurations.Rooms.Get().Select(room => new RoomStatusReactive()
        {
            NickName = room.NickName,
            RoomUrl = room.RoomUrl,
            IsToNotify = room.IsToNotify,
            IsToRecord = room.IsToRecord,
        }));

        WeakReferenceMessenger.Default.Register<RecMessage>(this, (_, msg) =>
        {
            ReloadRoomStatus();
        });

        DispatcherTimer.Tick += (_, _) =>
        {
            ReloadRoomStatus();
        };
        DispatcherTimer.Start();

        // TODO
        _ = Task.Run(async () => await GlobalMonitor.AttachConsolePeriodicTimer.AttachChildProcessAsync());
        _ = Task.Run(async () => await GlobalMonitor.StartAsync());
    }

    private void ReloadRoomStatus()
    {
        foreach (RoomStatus roomStatus in GlobalMonitor.RoomStatus.Values.ToArray())
        {
            RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == roomStatus.RoomUrl).FirstOrDefault();

            if (roomStatusReactive != null)
            {
                roomStatusReactive.StreamStatus = roomStatus.StreamStatus;
                roomStatusReactive.RecordStatus = roomStatus.RecordStatus;
                roomStatusReactive.HlsUrl = roomStatus.HlsUrl;
            }
        }
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

                rooms.RemoveAll(room => room.RoomUrl == dialog.Url);
                rooms.Add(new Room()
                {
                    NickName = dialog.NickName,
                    RoomUrl = dialog.RoomUrl!,
                });
                Configurations.Rooms.Set([.. rooms]);
                ConfigurationManager.Save();

                RoomStatuses.Add(new RoomStatusReactive()
                {
                    NickName = dialog.NickName,
                    RoomUrl = dialog.RoomUrl!,
                });
            }
        }
    }

    [RelayCommand]
    private void OpenSettingsDialog()
    {
        foreach (Window win in Application.Current.Windows.OfType<SettingsWindow>())
        {
            win.Close();
        }

        _ = new SettingsWindow()
        {
            Owner = Application.Current.MainWindow,
        }.ShowDialog();
    }

    [RelayCommand]
    private async Task OpenSaveFolderAsync()
    {
        await Launcher.LaunchFolderAsync(
            await StorageFolder.GetFolderFromPathAsync(
                SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get())
            )
        );
    }

    [RelayCommand]
    private async Task OpenSettingsFileFolderAsync()
    {
        await $"explorer"
            .WithArguments($"/select,\"{ConfigurationManager.FilePath}\"")
            .ExecuteAsync();
    }

    [RelayCommand]
    private async Task OpenAboutAsync()
    {
        AboutContentDialog dialog = new();
        _ = await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task PlayRecordAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task RowUpRoomUrlAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task RowDownRoomUrlAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task RemoveRoomUrlAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GotoRoomUrlAsync()
    {
        if (SelectedItem != null)
        {
            if (!string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
            {
                await Launcher.LaunchUriAsync(new Uri(SelectedItem.RoomUrl));
            }
        }
    }

    [RelayCommand]
    private async Task StopRecordAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private void IsToNotify()
    {
        if (SelectedItem != null)
        {
            if (!string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
            {
                RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault();

                if (roomStatusReactive != null)
                {
                    roomStatusReactive.IsToNotify = SelectedItem.IsToNotify;
                }

                Room[] rooms = Configurations.Rooms.Get();
                Room? room = rooms.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault();

                if (room != null)
                {
                    room.IsToNotify = SelectedItem.IsToNotify;
                }
                Configurations.Rooms.Set(rooms);
                ConfigurationManager.Save();
            }
        }
    }

    [RelayCommand]
    private void IsToRecord()
    {
        if (SelectedItem != null)
        {
            if (!string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
            {
                RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault();

                if (roomStatusReactive != null)
                {
                    roomStatusReactive.IsToRecord = SelectedItem.IsToRecord;
                }

                Room[] rooms = Configurations.Rooms.Get();
                Room? room = rooms.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault();

                if (room != null)
                {
                    room.IsToRecord = SelectedItem.IsToRecord;
                }
                Configurations.Rooms.Set(rooms);
                ConfigurationManager.Save();
            }
        }
    }

    [RelayCommand]
    private void OnContextMenuLoaded(RelayEventParameter param)
    {
        ContextMenu sender = (ContextMenu)param.Deconstruct().Sender;

        sender.Opened -= ContextMenuOpened;
        sender.Opened += ContextMenuOpened;

        // Closure method
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ContextMenuOpened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu { } contextMenu
             && contextMenu.Parent is Popup { } popup
             && popup.PlacementTarget is DataGrid { } dataGrid)
            {
                if (dataGrid.InputHitTest(Mouse.GetPosition(dataGrid)) is FrameworkElement { } element)
                {
                    if (GetDataGridRow(element) is DataGridRow { } row)
                    {
                        if (row.DataContext is RoomStatusReactive { } data)
                        {
                            _ = data.MapTo(SelectedItem);

                            foreach (UIElement d in ((ContextMenu)sender).Items.OfType<UIElement>())
                            {
                                d.Visibility = Visibility.Visible;
                            }
                        }
                    }
                    else
                    {
                        ((ContextMenu)sender).IsOpen = false;
                        _ = SelectedItem.MapFrom(new RoomStatusReactive());

                        foreach (UIElement d in ((ContextMenu)sender).Items.OfType<UIElement>())
                        {
                            d.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static DataGridRow? GetDataGridRow(FrameworkElement? element)
            {
                while (element != null && element is not DataGridRow)
                {
                    element = VisualTreeHelper.GetParent(element) as FrameworkElement;
                }
                return element as DataGridRow;
            }
        }
    }
}
