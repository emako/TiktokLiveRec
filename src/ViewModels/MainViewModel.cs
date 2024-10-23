using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using Flucli;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using TiktokLiveRec.Views;
using Windows.Storage;
using Windows.System;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;

namespace TiktokLiveRec;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
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

    public DataGrid RecTable { get; private set; } = null!;

    [ObservableProperty]
    private ObservableCollection<RecRoom> recs = [];

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

                Recs.Add(new RecRoom()
                {
                    NickName = dialog.NickName,
                    LiveUrl = dialog.Url!,
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
}

public partial class RecRoom : ObservableObject
{
    [ObservableProperty]
    private string nickName = string.Empty;

    [ObservableProperty]
    private string liveUrl = string.Empty;
}
