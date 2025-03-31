using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System.Diagnostics.CodeAnalysis;
using UrsaAvaloniaUI.Platform.Windows;

namespace TiktokLiveRec;

public partial class App : Application
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public override void Initialize()
    {
        // Apply Windows system features.
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            //_ = DpiAware.SetProcessDpiAwareness();
            WindowSystemMenu.ApplySystemMenuTheme(isDark: Current?.ActualThemeVariant == ThemeVariant.Dark);
        }

        AvaloniaXamlLoader.Load(this);
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
