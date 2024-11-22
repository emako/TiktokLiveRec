using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using Fischless.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using Windows.Storage;
using Windows.System;
using WindowsAPICodePack.Dialogs;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;
using Wpf.Ui.Violeta.Controls;
using Wpf.Ui.Violeta.Resources;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class SettingsViewModel : ReactiveObject
{
    private enum LanguageIndexEnum
    {
        Auto,
        ChineseSimplified,
        ChineseTraditional,
        English,
        Japanese,
    }

    [ObservableProperty]
    private int languageIndex = Configurations.Language.Get() switch
    {
        "zh" or "zh-Hans" => (int)LanguageIndexEnum.ChineseSimplified,
        "zh-Hant" => (int)LanguageIndexEnum.ChineseTraditional,
        "en" => (int)LanguageIndexEnum.English,
        "ja" => (int)LanguageIndexEnum.Japanese,
        _ => (int)LanguageIndexEnum.Auto,
    };

    partial void OnLanguageIndexChanged(int value)
    {
        string language = value switch
        {
            (int)LanguageIndexEnum.ChineseSimplified => "zh-Hans",
            (int)LanguageIndexEnum.ChineseTraditional => "zh-Hant",
            (int)LanguageIndexEnum.English => "en",
            (int)LanguageIndexEnum.Japanese => "ja",
            _ => string.Empty,
        };

        Locale.Culture = value switch
        {
            (int)LanguageIndexEnum.Auto => new CultureInfo(Interop.GetUserDefaultLocaleName()),
            _ => new CultureInfo(language),
        };

        Configurations.Language.Set(language);
        ConfigurationManager.Save();
    }

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
        TrayIconManager.GetInstance().UpdateTrayIcon();
    }

    [RelayCommand]
    private void CreateDesktopShortcut()
    {
        ShortcutHelper.CreateShortcutOnDesktop(
            shortcutName: "TiktokLiveRec",
            targetPath: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName),
            arguments: null!,
            description: "Title".Tr(),
            iconLocation: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".exe"));

        Toast.Success("SuccOp".Tr());
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
    private string toNotifyWithEmailUserName = Configurations.ToNotifyWithEmailUserName.Get();

    partial void OnToNotifyWithEmailUserNameChanged(string value)
    {
        Configurations.ToNotifyWithEmailUserName.Set(value);
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
    private bool isToNotifyGotoRoomUrl = Configurations.IsToNotifyGotoRoomUrl.Get();

    partial void OnIsToNotifyGotoRoomUrlChanged(bool value)
    {
        Configurations.IsToNotifyGotoRoomUrl.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isToNotifyGotoRoomUrlAndMute = Configurations.IsToNotifyGotoRoomUrlAndMute.Get();

    partial void OnIsToNotifyGotoRoomUrlAndMuteChanged(bool value)
    {
        Configurations.IsToNotifyGotoRoomUrlAndMute.Set(value);
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
    private int routineInterval = Configurations.RoutineInterval.Get();

    partial void OnRoutineIntervalChanged(int value)
    {
        GlobalMonitor.RoutinePeriodicWait.Period = TimeSpan.FromMilliseconds(int.Max(value, 500));
        Configurations.RoutineInterval.Set(value);
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
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private async Task OpenSaveFolderAsync()
    {
        await Launcher.LaunchFolderAsync(
            await StorageFolder.GetFolderFromPathAsync(
                SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get())
            )
        );
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

    [RelayCommand]
    private async Task CheckProxyUrlAsync()
    {
        if (string.IsNullOrWhiteSpace(ProxyUrl))
        {
            Toast.Error("ProxyErrorOfEmptyUrl".Tr());
            return;
        }

        if (!ProxyUrl.Contains(':'))
        {
            Toast.Error("ProxyErrorOfMissHostOrPort".Tr());
            return;
        }

        string[] proxy = ProxyUrl.Split(':');

        if (proxy.Length < 2)
        {
            Toast.Error("ProxyErrorOfFormat".Tr());
            return;
        }

        if (!IPAddress.TryParse(proxy[0], out IPAddress? address))
        {
            Toast.Error("ProxyErrorOfHostFormatError".Tr());
            return;
        }

        if (!int.TryParse(proxy[1], out int port))
        {
            Toast.Error("ProxyErrorOfPortFormatError".Tr());
            return;
        }

        if (port <= 0 || port > short.MaxValue)
        {
            Toast.Error("ProxyErrorOfPortOutOfRange".Tr());
            return;
        }

        HttpClientHandler httpClientHandler = new()
        {
            Proxy = new WebProxy(address.ToString(), port),
            UseProxy = true
        };

        using HttpClient httpClient = new(httpClientHandler);

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://www.google.com");
            response.EnsureSuccessStatusCode();

            Toast.Success("ProxySuccOfStatusCode".Tr(response.StatusCode));
        }
        catch (HttpRequestException e)
        {
            Toast.Success("ProxyErrorOfExceptionMessage".Tr(e.Message));
        }
    }

    [ObservableProperty]
    private string cookieChina = Configurations.CookieChina.Get();

    partial void OnCookieChinaChanged(string value)
    {
        Configurations.CookieChina.Set(value);
        ConfigurationManager.Save();
    }

    [RelayCommand]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
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
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void OpenHowToGetCookieOversea()
    {
        // TODO
        Toast.Warning("ComingSoon".Tr() + " ...");
    }

    [ObservableProperty]
    private string userAgent = Configurations.UserAgent.Get();

    partial void OnUserAgentChanged(string value)
    {
        Configurations.UserAgent.Set(value);
        ConfigurationManager.Save();
    }
}
