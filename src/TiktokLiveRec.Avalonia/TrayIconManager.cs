using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Violeta.Platform.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TiktokLiveRec;

internal partial class TrayIconManager
{
    private static TrayIconManager _instance = null!;

    private readonly TrayIcon _icon = null!;

    [Description("Only for Windows")]
    private readonly TrayIconHost? _iconHost = null;

    public bool IsShutdownTriggered { get; private set; } = false;

    [SuppressMessage("Interoperability", "CA1416: Validate platform compatibility")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    private TrayIconManager()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            using Icon icon = new(AssetLoader.Open(new Uri("avares://TiktokLiveRec/Assets/Favicon.ico")));

            _iconHost = new TrayIconHost()
            {
                ToolTipText = "TiktokLiveRec",
                Icon = icon.Handle,
                Menu =
                [
                    new TrayMenuItem()
                    {
                        Header = Version,
                        IsEnabled = false,
                    },
                    new TraySeparator(),
                    new TrayMenuItem()
                    {
                        Header = "TrayMenuShowMainWindow".Tr(),
                        Command = ActivateMainWindowCommand,
                    },
                    new TrayMenuItem()
                    {
                        Header = "TrayMenuOpenSettings".Tr(),
                        Command = OpenSettingsCommand,
                    },
                    new TrayMenuItem()
                    {
                        Header = "TrayMenuAutoRun".Tr(),
                        Command = ToggleAutoRunCommand,
                    },
                    new TrayMenuItem()
                    {
                        Header = "TrayMenuRestart".Tr(),
                        Command = RestartCommand,
                    },
                    new TrayMenuItem()
                    {
                        Header = "TrayMenuExit".Tr(),
                        Command = ExitCommand,
                    }
                ],
            };

            _iconHost.LeftDoubleClick += (_, _) => ActivateOrRestoreMainWindow();
            _iconHost.Menu!.Opening += (_, _) =>
            {
                foreach (var item in _iconHost.Menu?.OfType<TrayMenuItem>() ?? [])
                {
                    if (item.Header == "TrayMenuAutoRun".Tr())
                    {
                        //item!.IsChecked = AutoStartupHelper.IsAutorun();
                    }
                }
            };
        }
        else
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
                    new NativeMenuItemSeparator(),
                    new NativeMenuItem()
                    {
                        Header = "TrayMenuShowMainWindow".Tr(),
                        Command = ActivateMainWindowCommand,
                    },
                    new NativeMenuItem()
                    {
                        Header = "TrayMenuOpenSettings".Tr(),
                        Command = OpenSettingsCommand,
                    },
                    new NativeMenuItem()
                    {
                        Header = "TrayMenuAutoRun".Tr(),
                        Command = ToggleAutoRunCommand,
                    },
                    new NativeMenuItem()
                    {
                        Header = "TrayMenuRestart".Tr(),
                        Command = RestartCommand,
                    },
                    new NativeMenuItem()
                    {
                        Header = "TrayMenuExit".Tr(),
                        Command = ExitCommand,
                    }
                ]
            };

            _icon.Clicked += (_, _) => ActivateOrRestoreMainWindow();
        }

        UpdateTrayIcon();

        Locale.CultureChanged += (_, _) =>
        {
            dynamic menu = Environment.OSVersion.Platform == PlatformID.Win32NT ?
                _iconHost?.Menu! :
                _icon.Menu!;

            foreach (dynamic item in menu)
            {
                if (item.Header?.Contains("(&V)") ?? false)
                {
                    item.Header = "TrayMenuShowMainWindow".Tr();
                }
                else if (item.Header?.Contains("(&S)") ?? false)
                {
                    item.Header = "TrayMenuOpenSettings".Tr();
                }
                else if (item.Header?.Contains("(&A)") ?? false)
                {
                    item.Header = "TrayMenuAutoRun".Tr();
                }
                else if (item.Header?.Contains("(&R)") ?? false)
                {
                    item.Header = "TrayMenuRestart".Tr();
                }
                else if (item.Header?.Contains("(&E)") ?? false)
                {
                    item.Header = "TrayMenuExit".Tr();
                }
            }
        };
    }

    public void UpdateTrayIcon()
    {
        // TODO
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

[SuppressMessage("Performance", "CA1822:Mark members as static")]
internal partial class TrayIconManager : ObservableObject
{
    [ObservableProperty]
    private string version = $"v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}";

    [RelayCommand]
    private void ActivateMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is not null)
            {
                desktop.MainWindow.Show();
                desktop.MainWindow.Activate();
            }
        }
    }

    [RelayCommand]
    private void ActivateOrRestoreMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is not null)
            {
                if (desktop.MainWindow.IsVisible)
                {
                    desktop.MainWindow.Hide();
                }
                else
                {
                    desktop.MainWindow.Show();
                    desktop.MainWindow.Activate();
                }
            }
        }
    }

    [RelayCommand]
    private void OpenSettings()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //foreach (var win in desktop.Windows.OfType<SettingsWindow>())
            //{
            //    win.Close();
            //}
        }
    }

    [RelayCommand]
    private void ToggleAutoRun()
    {
    }

    [RelayCommand]
    private void Restart()
    {
        try
        {
            using Process process = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = GetExecutablePath(),
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = true,
                },
            };
            process.Start();
        }
        catch (Win32Exception)
        {
            return;
        }

        Process.GetCurrentProcess().Kill();

        static string GetExecutablePath()
        {
            string fileName = AppDomain.CurrentDomain.FriendlyName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                fileName += ".exe";

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
    }

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
