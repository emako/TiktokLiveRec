using MediaInfoLib;
using System.Diagnostics;
using System.IO;
using System.Windows.Resources;
using TiktokLiveRec.Extensions;

namespace TiktokLiveRec.Core;

internal sealed class Player
{
    public static async Task PlayAsync(string mediaPath)
    {
        if (!File.Exists(mediaPath))
        {
            return;
        }

        bool isFFplay = false;
        string player = Configurations.Player.Get() ?? string.Empty;
        bool isPlayerRect = Configurations.IsPlayerRect.Get();
        string? playerPath = null;
        string playerArgs = string.Empty;

        if (player.Equals("System", StringComparison.OrdinalIgnoreCase))
        {
            playerPath = "cmd";
            playerArgs = $"/c start \"{mediaPath}\"";
        }
        else
        {
            isFFplay = true;

            playerPath = SearchFileHelper.SearchFiles(".", "ffplay.exe").FirstOrDefault();

            using MediaInfo lib = new();
            lib.Open(mediaPath);

            if (double.TryParse(lib.Get(StreamKind.Video, 0, "Duration"), out double duration))
            {
                playerArgs = $"-ss {duration / 1000 - 5} -i \"{mediaPath}\"";
            }
            else
            {
                playerArgs = $"-i \"{mediaPath}\"";
            }
        }

        if (string.IsNullOrEmpty(playerPath))
        {
            _ = MessageBox.Warning("未找到文件 ffplay.exe 因此无法播放！");
            return;
        }

        using Process process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = playerPath,
                Arguments = playerArgs,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        process.Start();
        Debug.WriteLine($"{process.StartInfo.FileName} with arguments {process.StartInfo.Arguments} PID: {process.Id}");

        if (isFFplay)
        {
            await Task.Delay(1);
            _ = SpinWait.SpinUntil(() => process.HasExited || process.MainWindowHandle != IntPtr.Zero, 30000);

            if (process.HasExited)
            {
                return;
            }

            foreach (nint hWnd in Interop.GetWindowHandleByProcessId(process.Id))
            {
                if (!Interop.IsDarkModeForWindow(hWnd))
                {
                    Interop.EnableDarkModeForWindow(hWnd);
                    Interop.SetRoundedCorners(hWnd, enable: !isPlayerRect);
                    Interop.SetWindowTitle(hWnd, new FileInfo(mediaPath).Name);
                    Interop.SetHideFromTaskBar(hWnd);

                    // FFplay will lost icon when maximize.
                    // Interop.SetWindowIcon(hWnd, new(GetStream("pack://application:,,,/TiktokLiveRec;component/Assets/Favicon.ico")));
                }
            }

            if (process.HasExited)
            {
                return;
            }
        }

        static Stream GetStream(string uriString)
        {
            Uri uri = new(uriString);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            return info?.Stream!;
        }
    }
}
