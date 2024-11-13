using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Views;
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
            Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule?.FileName!)!,
            Visible = true
        };
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
