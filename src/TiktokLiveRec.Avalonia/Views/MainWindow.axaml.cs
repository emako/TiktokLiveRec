using TiktokLiveRec.ViewModels;
using FluentAvalonia.UI.Controls;

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
