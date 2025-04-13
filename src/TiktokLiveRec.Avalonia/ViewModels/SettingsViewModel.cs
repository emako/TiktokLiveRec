using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using Fischless.Configuration;
using FluentAvaloniaUI.Violeta.Platform;
using System.Globalization;

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
}
