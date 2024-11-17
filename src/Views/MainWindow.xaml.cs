using System.ComponentModel;
using System.Windows;
using TiktokLiveRec.Core;
using TiktokLiveRec.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Violeta.Appearance;

namespace TiktokLiveRec.Views;

public partial class MainWindow : FluentWindow
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        if (Enum.TryParse(Configurations.Theme.Get(), out ApplicationTheme applicationTheme))
        {
            ThemeManager.Apply(applicationTheme);
        }
        else
        {
            ThemeManager.Apply(ApplicationTheme.Unknown);
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (!TrayIconManager.GetInstance().IsShutdownTriggered)
        {
            e.Cancel = true;
            Hide();

            if (!Configurations.IsOffRemindCloseToTray.Get())
            {
                Notifier.AddNoticeWithButton("Title".Tr(), "CloseToTrayHint".Tr(), "ButtonOfOffRemind".Tr(), args: [("OffRemindTheCloseToTrayHint", bool.TrueString)]);
            }
        }
    }
}
