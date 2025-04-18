using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using TiktokLiveRec.Extensions;

namespace TiktokLiveRec.Views;

public partial class AboutDialogContent : UserControl
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

    [RelayCommand]
    private static void OpenHyperlink()
    {
        UrlHelper.OpenUrl(AppConfig.Url);
    }
}
