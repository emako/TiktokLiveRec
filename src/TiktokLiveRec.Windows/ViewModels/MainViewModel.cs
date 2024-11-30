using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;
using TiktokLiveRec.Threading;
using TiktokLiveRec.Views;
using Vanara.PInvoke;
using Windows.Storage;
using Windows.System;
using Wpf.Ui.Violeta.Controls;
using Wpf.Ui.Violeta.Threading;

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

    [ObservableProperty]
    private bool statusOfIsToNotify = Configurations.IsToNotify.Get();

    [ObservableProperty]
    private bool statusOfIsToRecord = Configurations.IsToRecord.Get();

    [ObservableProperty]
    private bool statusOfIsUseProxy = Configurations.IsUseProxy.Get();

    [ObservableProperty]
    private bool statusOfIsUseKeepAwake = Configurations.IsUseKeepAwake.Get();

    [ObservableProperty]
    private bool statusOfIsUseAutoShutdown = Configurations.IsUseAutoShutdown.Get();

    [ObservableProperty]
    private string statusOfAutoShutdownTime = Configurations.AutoShutdownTime.Get();

    [ObservableProperty]
    private string statusOfRecordFormat = Configurations.RecordFormat.Get();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusOfRoutineIntervalWithUnit))]
    private int statusOfRoutineInterval = Configurations.RoutineInterval.Get();

    public string StatusOfRoutineIntervalWithUnit
    {
        get
        {
            if (StatusOfRoutineInterval > 60000d)
            {
                return $"{Math.Round(StatusOfRoutineInterval / 60000d, 1)}min";
            }
            else if (StatusOfRoutineInterval > 1000d)
            {
                return $"{StatusOfRoutineInterval / 1000d}s";
            }
            else
            {
                return $"{StatusOfRoutineInterval}ms";
            }
        }
    }

    [ObservableProperty]
    private bool isReadyToShutdown = false;

    public CancellationTokenSource? ShutdownCancellationTokenSource { get; private set; } = null;

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

        WeakReferenceMessenger.Default.Register<ToastNotificationActivatedMessage>(this, (_, msg) =>
        {
            string arguments = msg.EventArgs.Argument;

            if (!string.IsNullOrEmpty(arguments))
            {
                NameValueCollection parsedArgs = HttpUtility.ParseQueryString(arguments);

                if (parsedArgs["AutoShutdownCancel"] != null)
                {
                    ShutdownCancellationTokenSource?.Cancel();
                }
            }
        });

        GlobalMonitor.Start();
        ChildProcessTracerPeriodicTimer.Default.WhiteList = ["ffmpeg", "ffplay"];
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
                roomStatusReactive.AvatarThumbUrl = roomStatus.AvatarThumbUrl;
                roomStatusReactive.StreamStatus = roomStatus.StreamStatus;
                roomStatusReactive.RecordStatus = roomStatus.RecordStatus;
                roomStatusReactive.FlvUrl = roomStatus.FlvUrl;
                roomStatusReactive.HlsUrl = roomStatus.HlsUrl;
                roomStatusReactive.StartTime = roomStatus.Recorder.StartTime;
                roomStatusReactive.EndTime = roomStatus.Recorder.EndTime;
                roomStatusReactive.RefreshDuration();
            }
        }

        IsRecording = RoomStatuses.Any(roomStatusReactive => roomStatusReactive.RecordStatus == RecordStatus.Recording);

        StatusOfIsToNotify = Configurations.IsToNotify.Get();
        StatusOfIsToRecord = Configurations.IsToRecord.Get();
        StatusOfIsUseProxy = Configurations.IsUseProxy.Get();
        StatusOfIsUseKeepAwake = Configurations.IsUseKeepAwake.Get();
        StatusOfIsUseAutoShutdown = Configurations.IsUseAutoShutdown.Get();
        StatusOfAutoShutdownTime = Configurations.AutoShutdownTime.Get();
        StatusOfRecordFormat = Configurations.RecordFormat.Get();
        StatusOfRoutineInterval = Configurations.RoutineInterval.Get();

        if (StatusOfIsUseAutoShutdown && TimeSpan.TryParse(StatusOfAutoShutdownTime, out TimeSpan targetTime))
        {
            int timeOffset = (int)(DateTime.Now.TimeOfDay - targetTime).TotalSeconds;

            if (timeOffset >= 0 && timeOffset <= 60)
            {
                IsReadyToShutdown = true;
            }

            if (IsReadyToShutdown && !IsRecording)
            {
                if (ShutdownCancellationTokenSource == null)
                {
                    ShutdownCancellationTokenSource = new();

                    Notifier.AddNoticeWithButton("Title".Tr(), "AutoShutdownInTime".Tr(), [
                        new ToastContentButtonOption()
                            {
                                Content = "ButtonOfCancel".Tr(),
                                Arguments = [("AutoShutdownCancel", string.Empty)],
                                ActivationType = ToastActivationType.Foreground,
                            }
                    ]);

                    ApplicationDispatcher.BeginInvoke(async () =>
                    {
                        await Task.Delay(60000);

                        if (!ShutdownCancellationTokenSource.IsCancellationRequested && !IsRecording)
                        {
                            if (Debugger.IsAttached)
                            {
                                _ = MessageBox.Information("AutoShutdown".Tr());
                            }
                            else
                            {
                                _ = Interop.ExitWindowsEx(User32.ExitWindowsFlags.EWX_SHUTDOWN | User32.ExitWindowsFlags.EWX_FORCE);
                            }
                        }

                        ShutdownCancellationTokenSource = null;
                        IsReadyToShutdown = false;
                    });
                }
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
