using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using AvaloniaUI.Violeta.Platform.Windows;
using System.Diagnostics.CodeAnalysis;
using Ursa.Controls;

namespace AvaloniaUI.Violeta.Controls;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class FluentWindowHelper
{
    public static InputElement UseAsTitleBarForWindowFrame(this InputElement self, Window window, bool isLimitedTop = false)
    {
        if (window == null) return self;

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            self.PointerPressed += (_, e) =>
            {
                if (window == null) return;

                PointerPoint pointerPoint = e.GetCurrentPoint(window);
                bool isLimitedTopPassed = false;

                if (isLimitedTop)
                {
                    // ExtendClientAreaTitleBarHeightHint 32 is the default title bar height for us
                    if (pointerPoint.Properties.IsLeftButtonPressed && pointerPoint.Position.Y <= 32)
                    {
                        if (((UrsaWindow)window)?.RightContent is Layoutable rightContent && rightContent.IsVisible)
                        {
                            if (pointerPoint.Position.X < window.Bounds.Width - rightContent.Bounds.Width)
                            {
                                isLimitedTopPassed = true;
                            }
                        }
                        else
                        {
                            isLimitedTopPassed = true;
                        }
                    }
                }
                else
                {
                    isLimitedTopPassed = true;
                }

                if (isLimitedTopPassed)
                {
                    if (pointerPoint.Properties.IsLeftButtonPressed)
                    {
                        if (e.ClickCount == 2)
                        {
                            // Custom content requires double-click to maximize function
                            if (window!.WindowState == WindowState.Normal)
                            {
                                window.WindowState = WindowState.Maximized;
                            }
                            else if (window.WindowState == WindowState.Maximized)
                            {
                                window.WindowState = WindowState.Normal;
                            }
                        }
                        else
                        {
                            // Custom content requires window move drag function
                            window!.BeginMoveDrag(e);
                        }
                    }
                }
            };
        }

        return self;
    }

    public static InputElement UseAsTitleBarForWindowSystemMenu(this InputElement self, Window window)
    {
        if (window == null) return self;

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            self.PointerPressed += (_, e) =>
            {
                if (window == null) return;

                PointerPoint pointerPoint = e.GetCurrentPoint(window);

                // ExtendClientAreaTitleBarHeightHint 32 is the default title bar height for us
                if (pointerPoint.Properties.IsRightButtonPressed && pointerPoint.Position.Y <= 32)
                {
                    if (((UrsaWindow)window)?.RightContent is Layoutable rightContent && rightContent.IsVisible)
                    {
                        if (pointerPoint.Position.X < window.Bounds.Width - rightContent.Bounds.Width)
                        {
                            // Show system menu
                            WindowSystemMenu.ShowSystemMenu(window, e);
                        }
                    }
                    else
                    {
                        // Show system menu
                        WindowSystemMenu.ShowSystemMenu(window!, e);
                    }
                }
            };
        }

        return self;
    }
}
