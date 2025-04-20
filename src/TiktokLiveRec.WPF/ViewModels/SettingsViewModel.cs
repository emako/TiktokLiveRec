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
using Vanara.PInvoke;
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
        "TS/FLV -> MP4" => 1,
        "TS/FLV -> MKV" => 2,
        "TS/FLV" or _ => 0,
    };

    partial void OnRecordFormatIndexChanged(int value)
    {
        Configurations.RecordFormat.Set(value switch
        {
            1 => "TS/FLV -> MP4",
            2 => "TS/FLV -> MKV",
            0 or _ => "TS/FLV",
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
    private bool isToSegment = Configurations.IsToSegment.Get();

    partial void OnIsToSegmentChanged(bool value)
    {
        Configurations.IsToSegment.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private int segmentTime = Configurations.SegmentTime.Get();

    partial void OnSegmentTimeChanged(int value)
    {
        Configurations.SegmentTime.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private string saveFolder = Configurations.SaveFolder.Get();

    partial void OnSaveFolderChanged(string value)
    {
        Configurations.SaveFolder.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool saveFolderDistinguishedByAuthors = Configurations.SaveFolderDistinguishedByAuthors.Get();

    partial void OnSaveFolderDistinguishedByAuthorsChanged(bool value)
    {
        Configurations.SaveFolderDistinguishedByAuthors.Set(value);
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
        // TODO: Implement for other platforms
        await Launcher.LaunchFolderAsync(
            await StorageFolder.GetFolderFromPathAsync(
                SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get())
            )
        );
    }

    [ObservableProperty]
    private int playerIndex = Configurations.Player.Get() switch
    {
        "ffplay" => 0,
        "system" or _ => 1,
    };

    partial void OnPlayerIndexChanged(int value)
    {
        Configurations.Player.Set(value switch
        {
            0 => "ffplay",
            1 or _ => "system",
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
    private bool isUseKeepAwake = Configurations.IsUseKeepAwake.Get();

    partial void OnIsUseKeepAwakeChanged(bool value)
    {
        if (value)
        {
            // Start keep awake
            _ = Kernel32.SetThreadExecutionState(Kernel32.EXECUTION_STATE.ES_CONTINUOUS | Kernel32.EXECUTION_STATE.ES_SYSTEM_REQUIRED | Kernel32.EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }
        else
        {
            // Stop keep awake
            _ = Kernel32.SetThreadExecutionState(Kernel32.EXECUTION_STATE.ES_CONTINUOUS);
        }
        Configurations.IsUseKeepAwake.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private bool isUseAutoShutdown = Configurations.IsUseAutoShutdown.Get();

    partial void OnIsUseAutoShutdownChanged(bool value)
    {
        Configurations.IsUseAutoShutdown.Set(value);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private int autoShutdownTimeHour = Configurations.AutoShutdownTime.Get().Split(':')[0].IntParse(fallback: 0);

    partial void OnAutoShutdownTimeHourChanged(int value)
    {
        Configurations.AutoShutdownTime.Set($"{value:D2}:{AutoShutdownTimeMinute:D2}");
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private int autoShutdownTimeMinute = Configurations.AutoShutdownTime.Get().Split(':')[1].IntParse(fallback: 0);

    partial void OnAutoShutdownTimeMinuteChanged(int value)
    {
        Configurations.AutoShutdownTime.Set($"{AutoShutdownTimeHour:D2}:{value:D2}");
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
    private async Task OpenHowToGetCookieChinaAsync()
    {
        string html = ResourcesProvider.GetString("pack://application:,,,/TiktokLiveRec;component/Assets/GETCOOKIE_DOUYIN.html");
        string filePath = Path.GetFullPath(ConfigurationSpecialPath.GetPath("GETCOOKIE_DOUYIN.html", AppConfig.PackName));

        File.WriteAllText(filePath, html);

        // TODO: Implement for other platforms
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
    private async Task OpenHowToGetCookieOverseaAsync()
    {
        string html = ResourcesProvider.GetString("pack://application:,,,/TiktokLiveRec;component/Assets/GETCOOKIE_TIKTOK.html");
        string filePath = Path.GetFullPath(ConfigurationSpecialPath.GetPath("GETCOOKIE_TIKTOK.html", AppConfig.PackName));

        File.WriteAllText(filePath, html);

        // TODO: Implement for other platforms
        await Launcher.LaunchUriAsync(new Uri($"file://{filePath}"));
    }

    [ObservableProperty]
    private string userAgent = Configurations.UserAgent.Get();

    partial void OnUserAgentChanged(string value)
    {
        Configurations.UserAgent.Set(value);
        ConfigurationManager.Save();
    }
}

file static class Extensions
{
    public static int IntParse(this string value, int fallback = default)
    {
        if (int.TryParse(value, out int output))
        {
            return output;
        }
        return fallback;
    }
}
