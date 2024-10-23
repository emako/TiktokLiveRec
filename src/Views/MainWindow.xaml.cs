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
    }
}
