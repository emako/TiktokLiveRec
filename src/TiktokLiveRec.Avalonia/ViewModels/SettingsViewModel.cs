using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fischless.Configuration;
using FluentAvaloniaUI.Violeta.Platform;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TiktokLiveRec.Extensions;

namespace TiktokLiveRec.ViewModels;

public partial class SettingsViewModel : ObservableObject
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
            (int)LanguageIndexEnum.Auto =>
                Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? new CultureInfo(Interop.GetUserDefaultLocaleName())
                    : CultureInfo.InstalledUICulture,
            _ => new CultureInfo(language),
        };

        Configurations.Language.Set(language);
        ConfigurationManager.Save();
    }

    [ObservableProperty]
    private int themeIndex = Configurations.Theme.Get() switch
    {
        nameof(ThemeVariantEnum.Light) => (int)ThemeVariantEnum.Light,
        nameof(ThemeVariantEnum.Dark) => (int)ThemeVariantEnum.Dark,
        _ => (int)ThemeVariantEnum.Default,
    };

    partial void OnThemeIndexChanged(int value)
    {
        Application.Current!.RequestedThemeVariant = (ThemeVariantEnum)value switch
        {
            ThemeVariantEnum.Light => ThemeVariant.Light,
            ThemeVariantEnum.Dark => ThemeVariant.Dark,
            _ => PlatformTheme.AppsUseDarkTheme() switch
            {
                true => ThemeVariant.Dark,
                _ => ThemeVariant.Light,
            },
        };

        Configurations.Theme.Set((ThemeVariantEnum)value switch
        {
            ThemeVariantEnum.Light => nameof(ThemeVariantEnum.Light),
            ThemeVariantEnum.Dark => nameof(ThemeVariantEnum.Dark),
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

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    [Description("Only used for Windows")]
    [RelayCommand]
    private void CreateDesktopShortcut()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            ShortcutHelper.CreateShortcutOnDesktop(
                shortcutName: "TiktokLiveRec",
                targetPath: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName),
                arguments: null!,
                description: "Title".Tr(),
                iconLocation: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".exe"));

            Toast.Success("SuccOp".Tr());
        }
    }
}
