using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using TiktokLiveRec.Extensions;

namespace TiktokLiveRec.Views;

public sealed partial class AboutDialogContent : UserControl
{
    public AboutDialogContent()
    {
        Grid rootGrid = new()
        {
            RowDefinitions = new RowDefinitions("Auto,Auto")
        };
        Grid innerGrid = new()
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*")
        };

        Image image = new()
        {
            Height = 48,
            Source = new Bitmap(AssetLoader.Open(new Uri("avares://TiktokLiveRec/Assets/Favicon.png")))
        };
        Grid.SetColumn(image, 0);

        StackPanel textStack = new()
        {
            Margin = new Thickness(12, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 4,
        };
        Grid.SetColumn(textStack, 1);

        textStack.Children.Add(new TextBlock { Text = AppConfig.PackName });
        textStack.Children.Add(new TextBlock { Text = AppConfig.Version });

        innerGrid.Children.Add(image);
        innerGrid.Children.Add(textStack);

        Grid.SetRow(innerGrid, 0);
        rootGrid.Children.Add(innerGrid);

        Button link = new()
        {
            Content = AppConfig.Url,
            Margin = new Thickness(0, 8, 0, 0),
            Command = OpenHyperlinkCommand,
        };
        Grid.SetRow(link, 1);
        rootGrid.Children.Add(link);

        Content = rootGrid;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == ParentProperty)
        {
            if (Parent is ContentDialog dialog)
            {
                dialog.Title = "About".Tr();
                dialog.CloseButtonText = "ButtonOfClose".Tr();

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow is Window window)
                    {
                        _ = dialog.UseAsTitleBarForWindowFrame(window, true)
                            .UseAsTitleBarForWindowSystemMenu(window);
                    }
                }
            }
        }

        base.OnPropertyChanged(change);
    }

    [RelayCommand]
    private static void OpenHyperlink()
    {
        UrlHelper.OpenUrl(AppConfig.Url);
    }
}
