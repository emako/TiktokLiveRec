using MediaInfoLib;
using System.Diagnostics;
using TiktokLiveRec.Extensions;
using Vanara.PInvoke;
using Windows.System;
using Wpf.Ui.Violeta.Resources;

namespace TiktokLiveRec.Core;

public sealed class Player
{
    /// <summary>
    /// Method to play video/audio file use 3rd party player.
    /// </summary>
    /// <param name="mediaPath">Media file path</param>
    /// <param name="isSeekable">Only seekable for FFplay</param>
    public static async Task PlayAsync(string mediaPath, bool isSeekable = false)
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

        if (player.Equals("system", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Implement for other platforms
            await Launcher.LaunchUriAsync(new Uri(mediaPath));
            return;
        }
        else
        {
            isFFplay = true;

            playerPath = SearchFileHelper.SearchFiles(".", "ffplay[/.exe]").FirstOrDefault();

            if (isSeekable)
            {
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
            else
            {
                playerArgs = $"-i \"{mediaPath}\"";
            }
        }

        if (string.IsNullOrEmpty(playerPath))
        {
            _ = MessageBox.Warning("PlayerErrorOfFFplayNotFound".Tr());
            return;
        }

        using Process process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = playerPath,
                Arguments = playerArgs,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        process.Start();
        Debug.WriteLine($"{process.StartInfo.FileName} with arguments {process.StartInfo.Arguments} PID: {process.Id}");

        if (isFFplay)
        {
            await Task.Delay(700);

            // Not needed because of `ProcessWindowStyle.Hidden`.
            // await Task.Run(() => _ = SpinWait.SpinUntil(() => process.HasExited || process.MainWindowHandle != nint.Zero, 3000));

            if (process.HasExited)
            {
                return;
            }

            nint[] hWnds = Interop.GetWindowHandleByProcessId(process.Id);

            if (hWnds.Length == 0)
            {
                await Task.Delay(700);
                hWnds = Interop.GetWindowHandleByProcessId(process.Id);

                if (hWnds.Length == 0)
                {
                    // No fallback more.
                }
            }

            foreach (nint hWnd in hWnds)
            {
                // Not needed because of `ProcessWindowStyle.Hidden`.
                // _ = User32.ShowWindow(hWnd, ShowWindowCommand.SW_HIDE);

                if (!Interop.IsDarkModeForWindow(hWnd))
                {
                    Interop.EnableDarkModeForWindow(hWnd);
                }

                Interop.SetRoundedCorners(hWnd, enable: !isPlayerRect);
                Interop.SetWindowTitle(hWnd, new FileInfo(mediaPath).Name);
                Interop.SetHideFromTaskBar(hWnd);

                // FFplay will lost icon when maximize.
                Interop.SetWindowIcon(hWnd, new(ResourcesProvider.GetStream("pack://application:,,,/TiktokLiveRec;component/Assets/Favicon.ico")));

                // Fit the player window size and center it.
                Interop.SetWindowCenterRatio(hWnd, 0.98d);

                // Show the window first time here.
                _ = User32.ShowWindow(hWnd, ShowWindowCommand.SW_NORMAL);
            }

            if (process.HasExited)
            {
                return;
            }
        }
    }
}
