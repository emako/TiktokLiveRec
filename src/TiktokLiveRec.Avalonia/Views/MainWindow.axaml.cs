using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Remote.Protocol.Input;
using TiktokLiveRec.ViewModels;
using UrsaAvaloniaUI.Controls;

namespace TiktokLiveRec.Views;

public partial class MainWindow : FluentWindow
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();

        MoreButton.PointerPressed += MoreButton_Tapped;
    }

    private void MoreButton_Tapped(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint((Visual)sender!).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            Console.WriteLine("ÓÒ¼üµã»÷°´Å¥");

            MoreButton.ContextMenu?.Open();
        }
    }
}
