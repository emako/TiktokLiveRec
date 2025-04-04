using TiktokLiveRec.ViewModels;
using UrsaAvaloniaUI.Controls;

namespace TiktokLiveRec;

public partial class MainWindow : FluentWindow
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }
}
