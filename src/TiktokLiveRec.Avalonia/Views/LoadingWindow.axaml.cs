using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace TiktokLiveRec.Views;

public sealed partial class LoadingWindow : Window, IDisposable
{
    public LoadingWindow()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        Close();
    }

    public static LoadingWindow ShowAsSplash<TWin>(int millisecondsDelay = 1500) where TWin : Window, new()
    {
        LoadingWindow splashWindow = new()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
        };

        Dispatcher.UIThread.Invoke(async () =>
        {
            await Task.Delay(millisecondsDelay);

            using (splashWindow)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.MainWindow = new TWin();
                    desktop.MainWindow.Show();
                }
            }
        });
        return splashWindow;
    }
}

public partial class LoadingWindow
{
    public static LoadingWindow? Current { get; private set; } = null;

    public static LoadingWindow? ShowAsync()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Window window = desktop.Windows
                .Where(win => win.IsActive)
                .FirstOrDefault()
                ?? desktop.MainWindow
                ?? throw new SystemException("No basic window found");

            Current = new()
            {
                Width = window.Width,
                Height = window.Height,
                Position = window.Position,
            };

            Current.Show();
            return Current;
        }

        return null;
    }
}
