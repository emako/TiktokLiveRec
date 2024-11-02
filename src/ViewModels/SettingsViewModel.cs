using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using System.IO;
using Windows.Storage;
using Windows.System;
using WindowsAPICodePack.Dialogs;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;
using Wpf.Ui.Violeta.Resources;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class SettingsViewModel : ReactiveObject
{
    [ObservableProperty]
    private int themeIndex = Configurations.Theme.Get() switch
    {
        nameof(ApplicationTheme.Light) => (int)ApplicationTheme.Light,
        nameof(ApplicationTheme.Dark) => (int)ApplicationTheme.Dark,
        _ => (int)ApplicationTheme.Unknown,
    };

    partial void OnThemeIndexChanged(int value)
    {
        ThemeManager.Apply((ApplicationTheme)value);
        Configurations.Theme.Set((ApplicationTheme)value switch
        {
            ApplicationTheme.Light => nameof(ApplicationTheme.Light),
            ApplicationTheme.Dark => nameof(ApplicationTheme.Dark),
            _ => string.Empty,
        });
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isUseStatusTray = Configurations.IsUseStatusTray.Get();

    partial void OnIsUseStatusTrayChanged(bool value)
    {
        Configurations.IsUseStatusTray.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToNotify = Configurations.IsToNotify.Get();

    partial void OnIsToNotifyChanged(bool value)
    {
        Configurations.IsToNotify.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToNotifyWithSystem = Configurations.IsToNotifyWithSystem.Get();

    partial void OnIsToNotifyWithSystemChanged(bool value)
    {
        Configurations.IsToNotifyWithSystem.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToNotifyWithMusic = Configurations.IsToNotifyWithMusic.Get();

    partial void OnIsToNotifyWithMusicChanged(bool value)
    {
        Configurations.IsToNotifyWithMusic.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string? toNotifyWithMusicPath = Configurations.ToNotifyWithMusicPath.Get();

    partial void OnToNotifyWithMusicPathChanged(string? value)
    {
        Configurations.ToNotifyWithMusicPath.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToNotifyWithEmail = Configurations.IsToNotifyWithEmail.Get();

    partial void OnIsToNotifyWithEmailChanged(bool value)
    {
        Configurations.IsToNotifyWithEmail.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string toNotifyWithEmailSmtp = Configurations.ToNotifyWithEmailSmtp.Get();

    partial void OnToNotifyWithEmailSmtpChanged(string value)
    {
        Configurations.ToNotifyWithEmailSmtp.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string toNotifyWithEmailAccount = Configurations.ToNotifyWithEmailAccount.Get();

    partial void OnToNotifyWithEmailAccountChanged(string value)
    {
        Configurations.ToNotifyWithEmailSmtp.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string toNotifyWithEmailPassword = Configurations.ToNotifyWithEmailPassword.Get();

    partial void OnToNotifyWithEmailPasswordChanged(string value)
    {
        Configurations.ToNotifyWithEmailPassword.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToRecord = Configurations.IsToRecord.Get();

    partial void OnIsToRecordChanged(bool value)
    {
        Configurations.IsToRecord.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private int recordFormatIndex = Configurations.RecordFormat.Get() switch
    {
        "TS+MP4" => 1,
        "TS+MKV" => 2,
        "TS" or _ => 0,
    };

    partial void OnRecordFormatIndexChanged(int value)
    {
        Configurations.RecordFormat.Set(value switch
        {
            1 => "TS+MP4",
            2 => "TS+MKV",
            0 or _ => "TS",
        });
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isRemoveTs = Configurations.IsRemoveTs.Get();

    partial void OnIsRemoveTsChanged(bool value)
    {
        Configurations.IsRemoveTs.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string saveFolder = Configurations.SaveFolder.Get();

    partial void OnSaveFolderChanged(string value)
    {
        Configurations.SaveFolder.Set(value);
        ConfigurationManager.Save();
    }

    [RelayCommand]
    private void SelectSaveFolder()
    {
        using CommonOpenFileDialog dialog = new()
        {
            IsFolderPicker = true,
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            SaveFolder = dialog.FileName;
        }
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

    [ObservableProperty]
    private int playerIndex = Configurations.Player.Get() switch
    {
        "FFplay" => 1,
        "System" or _ => 0,
    };

    partial void OnPlayerIndexChanged(int value)
    {
        Configurations.Player.Set(value switch
        {
            1 => "FFplay",
            0 or _ => string.Empty,
        });
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isPlayerRect = Configurations.IsPlayerRect.Get();

    partial void OnIsPlayerRectChanged(bool value)
    {
        Configurations.IsPlayerRect.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isUseProxy = Configurations.IsUseProxy.Get();

    partial void OnIsUseProxyChanged(bool value)
    {
        Configurations.IsUseProxy.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string proxyUrl = Configurations.ProxyUrl.Get();

    partial void OnProxyUrlChanged(string value)
    {
        Configurations.ProxyUrl.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string cookieChina = Configurations.CookieChina.Get();

    partial void OnCookieChinaChanged(string value)
    {
        Configurations.CookieChina.Set(value);
        ConfigurationManager.Save();
    }

    [RelayCommand]
    private async Task OpenHowToGetCookieChinaAsync()
    {
        string html = ResourcesProvider.GetString("pack://application:,,,/TiktokLiveRec;component/Assets/GETCOOKIE.html");
        string filePath = Path.GetFullPath(ConfigurationSpecialPath.GetPath("GETCOOKIE.html", AppConfig.PackName));

        File.WriteAllText(filePath, html);
        await Launcher.LaunchUriAsync(new Uri($"file://{filePath}"));
    }

    [ObservableProperty]
    private string cookieOversea = Configurations.CookieOversea.Get();

    partial void OnCookieOverseaChanged(string value)
    {
        Configurations.CookieOversea.Set(value);
        ConfigurationManager.Save();
    }

    [RelayCommand]
    private void OpenHowToGetCookieOversea()
    {
        // TODO
    }
}
