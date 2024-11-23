using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Threading;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;
using Wpf.Ui.Violeta.Controls;
using DataGrid = System.Windows.Controls.DataGrid;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    protected internal ForeverDispatcherTimer DispatcherTimer { get; }

    [ObservableProperty]
    private ReactiveCollection<RoomStatusReactive> roomStatuses = [];

    [ObservableProperty]
    private RoomStatusReactive selectedItem = new();

    [ObservableProperty]
    private bool isRecording = false;

    partial void OnIsRecordingChanged(bool value)
    {
        TrayIconManager.GetInstance().UpdateTrayIcon();
    }

    public MainViewModel()
    {
        DispatcherTimer = new(TimeSpan.FromSeconds(3), ReloadRoomStatus);

        RoomStatuses.Reset(Configurations.Rooms.Get().Select(room => new RoomStatusReactive()
        {
            NickName = room.NickName,
            RoomUrl = room.RoomUrl,
            IsToNotify = room.IsToNotify,
            IsToRecord = room.IsToRecord,
        }));

        Locale.CultureChanged += (_, _) =>
        {
            foreach (RoomStatusReactive roomStatusReactive in RoomStatuses)
            {
                roomStatusReactive.RefreshStatus();
            }
        };

        GlobalMonitor.Start();
        ChildProcessTracerPeriodicTimer.Default.Start();
        DispatcherTimer.Start();
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
                roomStatusReactive.StartTime = roomStatus.Recorder.StartTime;
                roomStatusReactive.EndTime = roomStatus.Recorder.EndTime;
                roomStatusReactive.RefreshDuration();
            }
        }

        IsRecording = RoomStatuses.Any(roomStatusReactive => roomStatusReactive.RecordStatus == RecordStatus.Recording);
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
        // TODO: Implement for other platforms
        await Launcher.LaunchFolderAsync(
            await StorageFolder.GetFolderFromPathAsync(
                SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get())
            )
        );
    }

    [RelayCommand]
    private async Task OpenSettingsFileFolderAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            await "explorer"
                .WithArguments($"/select,\"{ConfigurationManager.FilePath}\"")
                .ExecuteAsync();
        }
        else
        {
            // TODO: Implement for other platforms
            await Launcher.LaunchUriAsync(new Uri(ConfigurationManager.FilePath));
        }
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
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        if (GlobalMonitor.RoomStatus.TryGetValue(SelectedItem.RoomUrl, out RoomStatus? roomStatus)
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
    private void RowUpRoomUrl()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        // SelectedItem's properties is mapped from CollectionView, so we need to find the original item
        RoomStatuses.MoveUp(RoomStatuses.Where(roomStatus => roomStatus.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault()!);
    }

    [RelayCommand]
    private void RowDownRoomUrl()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        // SelectedItem's properties is mapped from CollectionView, so we need to find the original item
        RoomStatuses.MoveDown(RoomStatuses.Where(roomStatus => roomStatus.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault()!);
    }

    [RelayCommand]
    private async Task RemoveRoomUrlAsync()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        MessageBoxResult result = await MessageBox.QuestionAsync("SureRemoveRoom".Tr(SelectedItem.NickName));

        if (result == MessageBoxResult.Yes)
        {
            // Stop and remove from Global status
            if (GlobalMonitor.RoomStatus.TryGetValue(SelectedItem.RoomUrl, out RoomStatus? roomStatus))
            {
                roomStatus.Recorder.Stop();
                _ = GlobalMonitor.RoomStatus.TryRemove(SelectedItem.RoomUrl, out _);
            }

            // Remove from Reactive UI
            RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == roomStatus?.RoomUrl).FirstOrDefault();
            if (roomStatusReactive != null)
            {
                RoomStatuses.Remove(roomStatusReactive);
            }

            // Remove from Configuration
            List<Room> rooms = [.. Configurations.Rooms.Get()];

            rooms.Remove(rooms.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault()!);
            Configurations.Rooms.Set([.. rooms]);
            ConfigurationManager.Save();

            Toast.Success("SuccOp".Tr());
        }
    }

    [RelayCommand]
    private async Task GotoRoomUrlAsync()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        // TODO: Implement for other platforms
        await Launcher.LaunchUriAsync(new Uri(SelectedItem.RoomUrl));
    }

    [RelayCommand]
    private async Task StopRecordAsync()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        if (GlobalMonitor.RoomStatus.TryGetValue(SelectedItem.RoomUrl, out RoomStatus? roomStatus))
        {
            if (roomStatus.RecordStatus == RecordStatus.Recording)
            {
                MessageBoxResult result = await MessageBox.QuestionAsync("SureStopRecord".Tr(roomStatus.NickName));

                if (result == MessageBoxResult.Yes)
                {
                    roomStatus.Recorder.Stop();
                    Toast.Success("SuccOp".Tr());
                }
            }
            else
            {
                Toast.Warning("NoRecordTask".Tr());
            }
        }
        else
        {
            Toast.Warning("NoRecordTask".Tr());
        }
    }

    [RelayCommand]
    private void ShowRecordLog()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        // TODO
        Toast.Warning("ComingSoon".Tr() + " ...");
    }

    [RelayCommand]
    private void IsToNotify()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

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

    [RelayCommand]
    private void IsToRecord()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

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

public static class ObservableCollectionExtensions
{
    public static void MoveUp<T>(this ObservableCollection<T> collection, T item)
    {
        int index = collection.IndexOf(item);

        if (index <= 0)
        {
            return;
        }
        collection.Move(index, index - 1);
    }

    public static void MoveDown<T>(this ObservableCollection<T> collection, T item)
    {
        int index = collection.IndexOf(item);

        if (index < 0 || index >= collection.Count - 1)
        {
            return;
        }
        collection.Move(index, index + 1);
    }
}
