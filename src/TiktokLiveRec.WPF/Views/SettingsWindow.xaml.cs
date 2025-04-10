using TiktokLiveRec.ViewModels;
using Wpf.Ui.Controls;

namespace TiktokLiveRec.Views;

public partial class SettingsWindow : FluentWindow
{
    public SettingsViewModel ViewModel { get; }

    public SettingsWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }
}
