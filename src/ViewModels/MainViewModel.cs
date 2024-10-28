using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;
using DataGrid = System.Windows.Controls.DataGrid;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    private DispatcherTimer timer = new();

    [ObservableProperty]
    private bool isThemeAuto = string.IsNullOrWhiteSpace(Configurations.Theme.Get());

    partial void OnIsThemeAutoChanged(bool value)
    {
        if (value)
        {
            ThemeManager.Apply(ApplicationTheme.Unknown);
            Configurations.Theme.Set(string.Empty);
            ConfigurationManager.Save();
        }
    }

    [ObservableProperty]
    private bool isThemeLight = Configurations.Theme.Get().Equals("Light");

    partial void OnIsThemeLightChanged(bool value)
    {
        if (value)
        {
            ThemeManager.Apply(ApplicationTheme.Light);
            Configurations.Theme.Set("Light");
            ConfigurationManager.Save();
        }
    }

    [ObservableProperty]
    private bool isThemeDark = Configurations.Theme.Get().Equals("Dark");

    partial void OnIsThemeDarkChanged(bool value)
    {
        if (value)
        {
            ThemeManager.Apply(ApplicationTheme.Dark);
            Configurations.Theme.Set("Dark");
            ConfigurationManager.Save();
        }
    }

    [ObservableProperty]
    private ReactiveCollection<RecRoom> recs = [];

    public MainViewModel()
    {
        Recs.Reset(Configurations.Rooms.Get().Select(room => new RecRoom()
        {
            NickName = room.NickName,
            RoomUrl = room.RoomUrl,
        }));

        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += async (sender, e) =>
        {
        };
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

                Recs.Add(new RecRoom()
                {
                    NickName = dialog.NickName,
                    RoomUrl = dialog.RoomUrl!,
                });
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
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GotoRoomUrlAsync()
    {
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task StopRecordAsync()
    {
        await Task.CompletedTask;
    }

    public DataGrid DataGrid { get; private set; } = null!;

    [RelayCommand]
    private void OnDataGridLoaded(RelayEventParameter param)
    {
        DataGrid = (DataGrid)param.Deconstruct().Sender;
    }

    [ObservableProperty]
    private RecRoom selectedItem = new();

    [ObservableProperty]
    private bool isToNotify = false;

    [ObservableProperty]
    private bool isToSpider = false;

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
                    if (row.DataContext is RecRoom { } data)
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
                    _ = SelectedItem.MapFrom(new RecRoom());

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
