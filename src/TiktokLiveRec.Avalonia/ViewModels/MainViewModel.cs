using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fischless.Configuration;
using Flucli;
using FluentAvalonia.UI.Controls;
using FluentAvaloniaUI.Violeta.Mvvm;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Threading;
using TiktokLiveRec.Views;

namespace TiktokLiveRec.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    internal ForeverDispatcherTimer DispatcherTimer { get; }

    [ObservableProperty]
    private ReactiveCollection<RoomStatusReactive> roomStatuses = [];

    [ObservableProperty]
    private RoomStatusReactive selectedItem = new();

    [ObservableProperty]
    private bool isRecording = false;

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

    partial void OnIsRecordingChanged(bool value)
    {
        TrayIconManager.GetInstance().UpdateTrayIcon();
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
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

        //TODO
        //WeakReferenceMessenger.Default.Register<ToastNotificationActivatedMessage>(this, (_, msg) =>
        //{
        //    string arguments = msg.EventArgs.Argument;

        //    if (!string.IsNullOrEmpty(arguments))
        //    {
        //        NameValueCollection parsedArgs = HttpUtility.ParseQueryString(arguments);

        //        if (parsedArgs["AutoShutdownCancel"] != null)
        //        {
        //            ShutdownCancellationTokenSource?.Cancel();
        //        }
        //    }
        //});

        //GlobalMonitor.Start();
        ChildProcessTracerPeriodicTimer.Default.WhiteList = ["ffmpeg", "ffplay"];
        ChildProcessTracerPeriodicTimer.Default.Start();
        DispatcherTimer.Start();
    }

    private void ReloadRoomStatus()
    {
        //TODO
        //foreach (RoomStatus roomStatus in GlobalMonitor.RoomStatus.Values.ToArray())
        //{
        //    RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == roomStatus.RoomUrl).FirstOrDefault();

        //    if (roomStatusReactive != null)
        //    {
        //        roomStatusReactive.AvatarThumbUrl = roomStatus.AvatarThumbUrl;
        //        roomStatusReactive.StreamStatus = roomStatus.StreamStatus;
        //        roomStatusReactive.RecordStatus = roomStatus.RecordStatus;
        //        roomStatusReactive.FlvUrl = roomStatus.FlvUrl;
        //        roomStatusReactive.HlsUrl = roomStatus.HlsUrl;
        //        roomStatusReactive.StartTime = roomStatus.Recorder.StartTime;
        //        roomStatusReactive.EndTime = roomStatus.Recorder.EndTime;
        //        roomStatusReactive.RefreshDuration();
        //    }
        //}

        //IsRecording = RoomStatuses.Any(roomStatusReactive => roomStatusReactive.RecordStatus == RecordStatus.Recording);

        //StatusOfIsToNotify = Configurations.IsToNotify.Get();
        //StatusOfIsToRecord = Configurations.IsToRecord.Get();
        //StatusOfIsUseProxy = Configurations.IsUseProxy.Get();
        //StatusOfIsUseKeepAwake = Configurations.IsUseKeepAwake.Get();
        //StatusOfIsUseAutoShutdown = Configurations.IsUseAutoShutdown.Get();
        //StatusOfAutoShutdownTime = Configurations.AutoShutdownTime.Get();
        //StatusOfRecordFormat = Configurations.RecordFormat.Get();
        //StatusOfRoutineInterval = Configurations.RoutineInterval.Get();

        //if (StatusOfIsUseAutoShutdown && TimeSpan.TryParse(StatusOfAutoShutdownTime, out TimeSpan targetTime))
        //{
        //    int timeOffset = (int)(DateTime.Now.TimeOfDay - targetTime).TotalSeconds;

        //    if (timeOffset >= 0 && timeOffset <= 60)
        //    {
        //        IsReadyToShutdown = true;
        //    }

        //    if (IsReadyToShutdown && !IsRecording)
        //    {
        //        if (ShutdownCancellationTokenSource == null)
        //        {
        //            ShutdownCancellationTokenSource = new();

        //            Notifier.AddNoticeWithButton("Title".Tr(), "AutoShutdownInTime".Tr(), [
        //                new ToastContentButtonOption()
        //                    {
        //                        Content = "ButtonOfCancel".Tr(),
        //                        Arguments = [("AutoShutdownCancel", string.Empty)],
        //                        ActivationType = ToastActivationType.Foreground,
        //                    }
        //            ]);

        //            ApplicationDispatcher.BeginInvoke(async () =>
        //            {
        //                await Task.Delay(60000);

        //                if (!ShutdownCancellationTokenSource.IsCancellationRequested && !IsRecording)
        //                {
        //                    if (Debugger.IsAttached)
        //                    {
        //                        _ = MessageBox.Information("AutoShutdown".Tr());
        //                    }
        //                    else
        //                    {
        //                        _ = Interop.ExitWindowsEx(User32.ExitWindowsFlags.EWX_SHUTDOWN | User32.ExitWindowsFlags.EWX_FORCE);
        //                    }
        //                }

        //                ShutdownCancellationTokenSource = null;
        //                IsReadyToShutdown = false;
        //            });
        //        }
        //    }
        //}
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
        UrlHelper.OpenUrl(SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get()));
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
            UrlHelper.OpenUrl(SaveFolderHelper.GetSaveFolder(ConfigurationManager.FilePath));
        }
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

    [RelayCommand]
    private async Task PlayRecordAsync()
    {
        await Task.CompletedTask;
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

        //if (result == MessageBoxResult.Yes)
        //{
        //    // Stop and remove from Global status
        //    if (GlobalMonitor.RoomStatus.TryGetValue(SelectedItem.RoomUrl, out RoomStatus? roomStatus))
        //    {
        //        roomStatus.Recorder.Stop();
        //        _ = GlobalMonitor.RoomStatus.TryRemove(SelectedItem.RoomUrl, out _);
        //    }

        //    // Remove from Reactive UI
        //    RoomStatusReactive? roomStatusReactive = RoomStatuses.Where(room => room.RoomUrl == roomStatus?.RoomUrl).FirstOrDefault();
        //    if (roomStatusReactive != null)
        //    {
        //        RoomStatuses.Remove(roomStatusReactive);
        //    }

        //    // Remove from Configuration
        //    List<Room> rooms = [.. Configurations.Rooms.Get()];

        //    rooms.Remove(rooms.Where(room => room.RoomUrl == SelectedItem.RoomUrl).FirstOrDefault()!);
        //    Configurations.Rooms.Set([.. rooms]);
        //    ConfigurationManager.Save();

        //    Toast.Success("SuccOp".Tr());
        //}
    }

    [RelayCommand]
    private void GotoRoomUrl()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        UrlHelper.OpenUrl(SelectedItem.RoomUrl);
    }

    [RelayCommand]
    private async Task StopRecordAsync()
    {
        if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.RoomUrl))
        {
            return;
        }

        //TODO
        //if (GlobalMonitor.RoomStatus.TryGetValue(SelectedItem.RoomUrl, out RoomStatus? roomStatus))
        //{
        //    if (roomStatus.RecordStatus == RecordStatus.Recording)
        //    {
        //        // https://github.com/emako/TiktokLiveRec/issues/13
        //        // https://github.com/emako/TiktokLiveRec/issues/19

        //        StackPanel content = new();
        //        CheckBox checkBox = new()
        //        {
        //            Content = "EnableRecord".Tr(),
        //            DataContext = SelectedItem,
        //        };

        //        // Do not use `CheckBox::Checked`, because it will be triggered when the CheckBox is loaded
        //        checkBox.Click += (_, _) =>
        //        {
        //            IsToRecord();
        //            Toast.Success("SuccOp".Tr());
        //        };

        //        // We not need to binding with two way, because we update the config through method `IsToRecord()`.
        //        checkBox.SetBinding(CheckBox.IsCheckedProperty, nameof(RoomStatusReactive.IsToRecord));

        //        content.Children.Add(new TextBlock()
        //        {
        //            Text = "SureStopRecord".Tr(roomStatus.NickName)
        //        });
        //        content.Children.Add(checkBox);

        //        ContentDialog dialog = new()
        //        {
        //            Title = "StopRecord".Tr(),
        //            Content = content,
        //            CloseButtonText = "ButtonOfCancel".Tr(),
        //            PrimaryButtonText = "StopRecord".Tr(),
        //            DefaultButton = ContentDialogButton.Primary,
        //        };

        //        ContentDialogResult result = await dialog.ShowAsync();

        //        if (result == ContentDialogResult.Primary)
        //        {
        //            roomStatus.Recorder.Stop();
        //            Toast.Success("SuccOp".Tr());
        //        }
        //    }
        //    else
        //    {
        //        Toast.Warning("NoRecordTask".Tr());
        //    }
        //}
        //else
        //{
        //    Toast.Warning("NoRecordTask".Tr());
        //}
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
}
