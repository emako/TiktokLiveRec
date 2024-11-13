using System.Windows;
using System.Windows.Threading;

namespace TiktokLiveRec.Views;

public partial class LoadingWindow : Window, IDisposable
{
    public LoadingWindow()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        Close();
    }
}

public partial class LoadingWindow
{
    public static Dispatcher? StaticDispatcher { get; protected set; } = null;
    public static bool Cancel { get; protected set; } = false;
    public static Rect Rectangle { get; protected set; } = default;

    public static LoadingWindowCloseDefer? ShowAsync()
    {
        if (StaticDispatcher != null)
        {
            return null;
        }

        Cancel = false;

        Window tar = Application.Current.MainWindow;
        Rectangle = new(tar.Left, tar.Top, tar.ActualWidth, tar.ActualHeight);

        Thread sta = new(Action)
        {
            IsBackground = true,
            Name = "STAThread<LoadingWindow>",
        };
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();

        return new LoadingWindowCloseDefer();

        static void Action()
        {
            // Had been canceled before thread launched
            if (Cancel)
            {
                return;
            }

            StaticDispatcher = Dispatcher.CurrentDispatcher;

            // Make it center of the target window
            using LoadingWindow win = new()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Width = Rectangle.Width,
                Height = Rectangle.Height,
                Left = Rectangle.Left,
                Top = Rectangle.Top,
                SizeToContent = SizeToContent.Manual,
            };

            win.ShowDialog();
        }
    }

    public static void CloseAsync()
    {
        Cancel = true;
        StaticDispatcher?.InvokeShutdown();
        StaticDispatcher = null;
    }
}

public sealed partial class LoadingWindowCloseDefer : IDisposable
{
    public void Dispose()
    {
        LoadingWindow.CloseAsync();
    }
}
