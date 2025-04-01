using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using System.Diagnostics.CodeAnalysis;
using Ursa.Controls;
using UrsaAvaloniaUI.Platform.Windows;

namespace UrsaAvaloniaUI.Controls;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class FluentWindow : UrsaWindow
{
    public FluentWindow()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            PointerPressed += OnFluentWindowPointerPressed;
            Deactivated += OnFluentWindowDeactivated;
            Activated += OnFluentWindowActivated;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }

    protected virtual void OnFluentWindowDeactivated(object? sender, EventArgs e)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
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
    }

    protected virtual void OnFluentWindowActivated(object? sender, EventArgs e)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            // Just case the window is activated, we will re-apply the backdrop
            TransparencyLevelHint = [WindowTransparencyLevel.Mica];
            Background = Brushes.Transparent;
        }
    }

    protected virtual void OnFluentWindowPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // ExtendClientAreaTitleBarHeightHint 32 is the default title bar height for us.
        if (e.GetCurrentPoint(this).Position.Y <= 32)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                // Show system menu
                WindowSystemMenu.ShowSystemMenu(this, e);
            }
        }
    }
}
