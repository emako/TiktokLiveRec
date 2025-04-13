using FluentAvalonia.UI.Controls;
using TiktokLiveRec.ViewModels;

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
