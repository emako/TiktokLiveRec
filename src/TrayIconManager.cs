using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;
using Wpf.Ui.Violeta.Resources;
using NotifyIcon = NotifyIconEx.NotifyIcon;

namespace TiktokLiveRec;

internal class TrayIconManager
{
    private static TrayIconManager _instance = null!;

    private readonly NotifyIcon _icon = null!;

    private readonly ToolStripMenuItem? _itemAutoRun = null;

    private TrayIconManager()
    {
        _icon = new NotifyIcon()
        {
            Text = "TiktokLiveRec",
            Visible = true
        };
        UpdateTrayIcon();
        _icon.AddMenu($"v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}").Enabled = false;
        _icon.AddMenu("-");
        _icon.AddMenu("TrayMenuShowMainWindow".Tr(), (_, _) =>
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.Activate();
            Interop.RestoreWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);
        });
        _icon.AddMenu("TrayMenuOpenSettings".Tr(), (_, _) =>
        {
            foreach (Window win in Application.Current.Windows.OfType<SettingsWindow>())
            {
                win.Close();
            }

            _ = new SettingsWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            }.ShowDialog();
        });
        _itemAutoRun = _icon.AddMenu("TrayMenuAutoRun".Tr(),
            (_, _) =>
            {
                if (AutoStartupHelper.IsAutorun())
                {
                    AutoStartupHelper.RemoveAutorunShortcut();
                }
                else
                {
                    AutoStartupHelper.CreateAutorunShortcut();
                }
            }) as ToolStripMenuItem;
        _icon.AddMenu("TrayMenuRestart".Tr(), (_, _) => RuntimeHelper.Restart(forced: true));
        _icon.AddMenu("TrayMenuExit".Tr(), (_, _) =>
        {
            Application.Current.Shutdown();
        });

        _icon.ContextMenuStrip.Opened += (_, _) =>
        {
            _itemAutoRun!.Checked = AutoStartupHelper.IsAutorun();
        };

        _icon.MouseDoubleClick += (_, _) =>
        {
            if (Application.Current.MainWindow.IsVisible)
            {
                Application.Current.MainWindow.Hide();
            }
            else
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
                Interop.RestoreWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            }
        };

        Locale.CultureChanged += (_, _) =>
        {
            foreach (ToolStripItem item in _icon.ContextMenuStrip.Items)
            {
                if (item.Text?.Contains("(&V)") ?? false)
                {
                    item.Text = "TrayMenuShowMainWindow".Tr();
                }
                else if (item.Text?.Contains("(&S)") ?? false)
                {
                    item.Text = "TrayMenuOpenSettings".Tr();
                }
                else if (item.Text?.Contains("(&A)") ?? false)
                {
                    item.Text = "TrayMenuAutoRun".Tr();
                }
                else if (item.Text?.Contains("(&R)") ?? false)
                {
                    item.Text = "TrayMenuRestart".Tr();
                }
                else if (item.Text?.Contains("(&E)") ?? false)
                {
                    item.Text = "TrayMenuExit".Tr();
                }
            }
        };

        SystemEvents.UserPreferenceChanged += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(Configurations.Theme.Get()))
            {
                ThemeManager.Apply(ApplicationTheme.Unknown);
            }
            UpdateTrayIcon();
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

    public void UpdateTrayIcon()
    {
        _icon.Icon = GetTrayIcon();

        static Icon GetTrayIcon()
        {
            try
            {
                if (Configurations.IsUseStatusTray.Get())
                {
                    string status = GlobalMonitor.RoomStatus.Values.ToArray().Any(roomStatus => roomStatus.RecordStatus == RecordStatus.Recording) ? "Recording" : "Unrecording";

                    SystemThemeManager.UpdateSystemThemeCache();
                    string theme = SystemThemeManager.GetCachedSystemTheme() switch
                    {
                        SystemTheme.Dark or SystemTheme.HCBlack or SystemTheme.Glow or SystemTheme.CapturedMotion => "Dark",
                        _ => "Light",
                    };

                    return new Icon(ResourcesProvider.GetStream($"pack://application:,,,/TiktokLiveRec;component/Assets/{status}{theme}.ico"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule?.FileName!)!;
        }
    }
}
