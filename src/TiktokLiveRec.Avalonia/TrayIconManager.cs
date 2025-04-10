using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace TiktokLiveRec;

internal partial class TrayIconManager
{
    private static TrayIconManager _instance = null!;

    private readonly TrayIcon _icon = null!;

    private TrayIconManager()
    {
        _icon = new()
        {
            ToolTipText = "TiktokLiveRec",
            Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://TiktokLiveRec/Assets/Favicon.png"))),
            Menu =
            [
                new NativeMenuItem()
                {
                    Header = Version,
                    IsEnabled = false,
                },
                new NativeMenuItem()
                {
                    Header = "Exit",
                    Command = ExitCommand,
                }
            ]
        };
    }

    public static TrayIconManager GetInstance()
    {
        return _instance ??= new TrayIconManager();
    }

    public static void Start()
    {
        _ = GetInstance();
    }
}

internal partial class TrayIconManager : ObservableObject
{
    [ObservableProperty]
    private string version = $"v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}";

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
