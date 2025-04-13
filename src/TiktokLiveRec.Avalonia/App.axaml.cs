using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Fischless.Configuration;
using FluentAvalonia.UI.Violeta.Platform.Windows;
using FluentAvaloniaUI.Violeta.Platform;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TiktokLiveRec.Views;

namespace TiktokLiveRec;

public partial class App : Application
{
    static App()
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);
        ConfigurationManager.ConfigurationSerializer = new YamlConfigurationSerializer();
        ConfigurationManager.Setup(ConfigurationSpecialPath.GetPath("config.yaml", AppConfig.PackName));
        Locale.Culture = string.IsNullOrWhiteSpace(Configurations.Language.Get()) ? CultureInfo.CurrentUICulture : new CultureInfo(Configurations.Language.Get());
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    public override void Initialize()
    {
        // Enable Hot Reload provided by HotAvalonia.
#if DEBUG
        HotAvalonia.AvaloniaHotReloadExtensions.EnableHotReload(this);
#endif

        // Apply Windows system features.
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            //_ = DpiAware.SetProcessDpiAwareness();
            WindowSystemMenu.ApplySystemMenuTheme(isDark: Current?.ActualThemeVariant.ToString() == nameof(ThemeVariant.Dark));
        }

        AvaloniaXamlLoader.Load(this);

        Current!.RequestedThemeVariant = Configurations.Theme.Get() switch
        {
            nameof(ThemeVariantEnum.Light) => ThemeVariant.Light,
            nameof(ThemeVariantEnum.Dark) => ThemeVariant.Dark,
            _ => PlatformTheme.AppsUseDarkTheme() switch
            {
                true => ThemeVariant.Dark,
                _ => ThemeVariant.Light,
            },
        };

        TrayIconManager.Start();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
