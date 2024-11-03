using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TiktokLiveRec.Core;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;
using DataGrid = System.Windows.Controls.DataGrid;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    private DispatcherTimer timer = new()
    {
        Interval = TimeSpan.FromSeconds(3)
    };

    [ObservableProperty]
    private ReactiveCollection<RoomStatusReactive> recs = [];

    public MainViewModel()
    {
        Recs.Reset(Configurations.Rooms.Get().Select(room => new RoomStatusReactive()
        {
            NickName = room.NickName,
            RoomUrl = room.RoomUrl,
        }));

        WeakReferenceMessenger.Default.Register<RecMessage>(this, (_, msg) =>
        {
            // TODO
        });

        timer.Tick += (object? sender, EventArgs e) =>
        {
            // TODO
        };
        timer.Start();

        _ = Task.Run(async () => await GlobalMonitor.StartAsync());
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

                Recs.Add(new RoomStatusReactive()
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
        string saveFolder = Configurations.SaveFolder.Get();

        if (string.IsNullOrWhiteSpace(saveFolder))
        {
            const string path = "download";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(Path.GetFullPath(path)));
        }
        else
        {
            await Launcher.LaunchFolderAsync(await StorageFolder.GetFolderFromPathAsync(Path.GetFullPath(saveFolder)));
        }
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
        // TODO
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task StopRecordAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    public DataGrid DataGrid { get; private set; } = null!;

    [RelayCommand]
    private void OnDataGridLoaded(RelayEventParameter param)
    {
        DataGrid = (DataGrid)param.Deconstruct().Sender;
    }

    [ObservableProperty]
    private RoomStatusReactive selectedItem = new();

    [ObservableProperty]
    private bool isToNotify = false;

    [ObservableProperty]
    private bool isToRecord = false;

    [RelayCommand]
    private void OnContextMenuLoaded(RelayEventParameter param)
    {
        ContextMenu sender = (ContextMenu)param.Deconstruct().Sender;

        sender.Opened -= ContextMenuOpened;
        sender.Opened += ContextMenuOpened;

        // Closure method
        void ContextMenuOpened(object sender, RoutedEventArgs e)
        {
            if (DataGrid.InputHitTest(Mouse.GetPosition(DataGrid)) is FrameworkElement { } element)
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
