using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System.Diagnostics.CodeAnalysis;
using Ursa.Controls;
using AvaloniaUI.Violeta.Platform.Windows;

namespace AvaloniaUI.Violeta.Controls;

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

        if (TitleBarContent is Control titleBar)
        {
            titleBar.PointerPressed += OnTitleBarPointerPressed;
        }
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
        PointerPoint pointerPoint = e.GetCurrentPoint(this);

        // ExtendClientAreaTitleBarHeightHint 32 is the default title bar height for us
        if (pointerPoint.Properties.IsRightButtonPressed && pointerPoint.Position.Y <= 32)
        {
            if (RightContent is Layoutable rightContent && rightContent.IsVisible)
            {
                if (pointerPoint.Position.X < Bounds.Width - rightContent.Bounds.Width)
                {
                    // Show system menu
                    WindowSystemMenu.ShowSystemMenu(this, e);
                }
            }
            else
            {
                // Show system menu
                WindowSystemMenu.ShowSystemMenu(this, e);
            }
        }
    }

    protected virtual void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        PointerPoint pointerPoint = e.GetCurrentPoint(this);

        if (pointerPoint.Properties.IsLeftButtonPressed)
        {
            if (e.ClickCount == 2)
            {
                // Custom content requires double-click to maximize function
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
            else
            {
                // Custom content requires window move drag function
                BeginMoveDrag(e);
            }
        }
    }
}
