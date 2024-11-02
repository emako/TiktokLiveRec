using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Views;
using NotifyIcon = NotifyIconEx.NotifyIcon;

namespace TiktokLiveRec;

internal class TrayIconManager
{
    private static TrayIconManager _instance = null!;

    private readonly NotifyIcon _icon = null!;

    private readonly ToolStripMenuItem? _itemAutoRun = null;

    private Process process = null!;
    private MemoryStream stream = new();

    private TrayIconManager()
    {
        //if (Kernel32.GetConsoleWindow() == 0)
        //{
        //    Kernel32.AllocConsole();
        //}

        //IEnumerable<string> result = SearchFileHelper.SearchFiles(".", "ffmpeg.exe");

        //process = new()
        //{
        //    StartInfo = new ProcessStartInfo()
        //    {
        //        FileName = result.FirstOrDefault(),
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        RedirectStandardInput = true,
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //    },
        //};
        //process.ErrorDataReceived += (sender, e) =>
        //{
        //    if (e.Data != null)
        //    {
        //        Console.WriteLine(e.Data);
        //    }
        //};
        //process.OutputDataReceived += (sender, e) =>
        //{
        //    if (e.Data != null)
        //    {
        //        Console.WriteLine(e.Data);
        //    }
        //};
        //process.Start();
        //process.BeginOutputReadLine();
        //process.BeginErrorReadLine();
        //Debug.WriteLine($"{process.StartInfo.FileName} PID: {process.Id}");

        _icon = new NotifyIcon()
        {
            Text = "Tiktok Live Rec",
            Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule?.FileName!)!,
            Visible = true
        };
        _icon.AddMenu($"v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}").Enabled = false;
        _icon.AddMenu("-");
        _icon.AddMenu("打开设置 (&O)", (_, _) =>
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
        _itemAutoRun = _icon.AddMenu("启动时自动运行 (&S)",
            (_, _) =>
            {
                if (AutoStartupHelper.IsAutorun())
                    AutoStartupHelper.RemoveAutorunShortcut();
                else
                    AutoStartupHelper.CreateAutorunShortcut();
            }) as ToolStripMenuItem;
        _icon.AddMenu("重启 (&R)", (_, _) => RuntimeHelper.Restart(forced: true));
        _icon.AddMenu("退出 (&E)", (_, _) =>
        {
            process?.Kill();
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
            }
        };
    }

    public static TrayIconManager GetInstance()
    {
        return _instance ??= new TrayIconManager();
    }

    public static Process Start()
    {
        return GetInstance().process;
    }
}
