using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Violeta.Platform.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TiktokLiveRec.Views;

namespace TiktokLiveRec;

public partial class App : Application
{
    static App()
    {
        Locale.Culture = CultureInfo.CurrentUICulture;
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
            WindowSystemMenu.ApplySystemMenuTheme(isDark: Current?.ActualThemeVariant == ThemeVariant.Dark);
        }

        AvaloniaXamlLoader.Load(this);

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
