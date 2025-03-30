using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using System.Diagnostics.CodeAnalysis;
using Ursa.Controls;
using UrsaAvaloniaUI.Platform.Windows;

namespace UrsaAvaloniaUI.Controls;

public class FluentWindow : UrsaWindow
{
    public FluentWindow()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Deactivated += OnFluentWindowDeactivated;
            Activated += OnFluentWindowActivated;
            Loaded += OnFluentWindowLoaded;
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    protected virtual void OnFluentWindowLoaded(object? sender, RoutedEventArgs e)
    {
        PointerPressed += (_, e) =>
        {
            if (e.GetCurrentPoint(this).Position.Y <= 32)
            {
                if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                {
                    // Show system menu
                    WindowSystemMenu.ShowSystemMenu(this, e);
                }
            }
        };
    }

    protected virtual void OnFluentWindowDeactivated(object? sender, EventArgs e)
    {
        ThemeVariant? themeVariant = Application.Current?.ActualThemeVariant;

        if (themeVariant == ThemeVariant.Dark)
        {
            Background = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        }
        else
        {
            Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
        }

        TransparencyLevelHint = [];
    }

    protected virtual void OnFluentWindowActivated(object? sender, EventArgs e)
    {
        // Just case the window is activated, we will re-apply the backdrop
        TransparencyLevelHint = [WindowTransparencyLevel.Mica];
        Background = Brushes.Transparent;
    }
}
