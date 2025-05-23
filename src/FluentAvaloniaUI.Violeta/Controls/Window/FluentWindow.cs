using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using System.Diagnostics.CodeAnalysis;
using Ursa.Controls;

namespace FluentAvalonia.UI.Controls;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class FluentWindow : UrsaWindow
{
    public FluentWindow()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            _ = this.UseAsTitleBarForWindowSystemMenu(this);

            Deactivated += OnFluentWindowDeactivated;
            Activated += OnFluentWindowActivated;

            ActualThemeVariantChanged += OnFluentWindowActualThemeVariantChanged;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (TitleBarContent is Control titleBar)
            {
                _ = titleBar.UseAsTitleBarForWindowFrame(this);
            }
        }
    }

    protected virtual void OnFluentWindowActualThemeVariantChanged(object? sender, EventArgs e)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            ThemeVariant? themeVariant = Application.Current?.ActualThemeVariant;

            if (themeVariant == ThemeVariant.Dark)
            {
                Background = new SolidColorBrush(Color.FromRgb(0x20, 0x20, 0x20));
            }
            else
            {
                Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
            }
        }
    }

    protected virtual void OnFluentWindowDeactivated(object? sender, EventArgs e)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            ThemeVariant? themeVariant = Application.Current?.ActualThemeVariant;

            if (themeVariant == ThemeVariant.Dark)
            {
                Background = new SolidColorBrush(Color.FromRgb(0x20, 0x20, 0x20));
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
}
