using TiktokLiveRec.ViewModels;
using AvaloniaUI.Violeta.Controls;

namespace TiktokLiveRec.Views;

public partial class MainWindow : FluentWindow
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }
}
