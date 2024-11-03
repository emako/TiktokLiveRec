using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MediaInfoLib;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Web;
using Windows.System;
using Wpf.Ui.Violeta.Resources;

namespace TiktokLiveRec.Core;

internal static class GlobalMonitor
{
    public static ConcurrentDictionary<string, RoomStatus> RoomStatus { get; } = new();

    private sealed class GlobalMonitorRecipient : ObservableRecipient
    {
        public static GlobalMonitorRecipient Instance { get; } = new();
    }

    static GlobalMonitor()
    {
        WeakReferenceMessenger.Default.Register<ToastNotificationActivatedMessage>(GlobalMonitorRecipient.Instance, async (_, msg) =>
        {
            string arguments = msg.EventArgs.Argument;

            if (!string.IsNullOrEmpty(arguments))
            {
                NameValueCollection parsedArgs = HttpUtility.ParseQueryString(arguments);

                if (parsedArgs["RoomUrl"] != null)
                {
                    try
                    {
                        await Launcher.LaunchUriAsync(new Uri(parsedArgs["RoomUrl"]!));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        });
    }

    public static async Task StartAsync(CancellationToken token = default)
    {
        using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(5000000));

        while (await timer.WaitForNextTickAsync(token).ConfigureAwait(false))
        {
            try
            {
                // Check Global Settings
                if (Configurations.IsToNotify.Get() || Configurations.IsToRecord.Get())
                {
                    Room[] rooms = Configurations.Rooms.Get();

                    foreach (Room room in rooms)
                    {
                        // Check Room Settings
                        if (room.IsToNotify || room.IsToRecord)
                        {
                            // First insert
                            if (!RoomStatus.ContainsKey(room.RoomUrl))
                            {
                                RoomStatus.TryAdd(room.RoomUrl, new RoomStatus()
                                {
                                    NickName = room.NickName,
                                    RoomUrl = room.RoomUrl,
                                    HlsUrl = null!,
                                    StreamStatus = StreamStatus.Initialized,
                                    RecordStatus = RecordStatus.Initialized,
                                });
                            }

                            SpiderResult spiderResult = Spider.GetResult(room.RoomUrl);

                            if (RoomStatus.TryGetValue(room.RoomUrl, out RoomStatus? roomStatus))
                            {
                                if (room.IsToNotify)
                                {
                                    if (roomStatus.StreamStatus != StreamStatus.Streaming && (spiderResult.IsLiveStreaming ?? false))
                                    {
                                        Notify(room, token);
                                    }
                                }
                                roomStatus.HlsUrl = spiderResult.HlsUrl!;
                            }

                            WeakReferenceMessenger.Default.Send(new RecMessage()
                            {
                                Type = RecMessage.RecMessageType.StartStreaming,
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    private static void Notify(Room room, CancellationToken token = default)
    {
        if (Configurations.IsToNotifyWithSystem.Get())
        {
            Notifier.AddNoticeWithButton("开播通知", room.NickName, "进入直播间", [("RoomUrl", room.RoomUrl)]);
        }

        if (Configurations.IsToNotifyWithMusic.Get())
        {
            _ = Task.Run(async () =>
            {
                const string musicPack = "pack://application:,,,/TiktokLiveRec;component/Assets/b_101.f1304dc4.mp3";
                string? musicPath = Configurations.ToNotifyWithMusicPath.Get();

                if (File.Exists(musicPath))
                {
                    using MediaInfo lib = new();
                    lib.Open(musicPath);
                    string audioTrackCount = lib.Get(StreamKind.Audio, 0, "StreamCount");

                    if (int.TryParse(audioTrackCount, out int count) && count > 0)
                    {
                        using FileStream stream = File.OpenRead(musicPath);
                        await Notifier.PlayMusicAsync(stream);
                    }
                    else
                    {
                        using Stream stream = ResourcesProvider.GetStream(musicPack);
                        await Notifier.PlayMusicAsync(stream);
                    }
                }
                else
                {
                    using Stream stream = ResourcesProvider.GetStream(musicPack);
                    await Notifier.PlayMusicAsync(stream);
                }
            }, token);
        }

        if (Configurations.IsToNotifyWithEmail.Get())
        {
            string smtpServer = Configurations.ToNotifyWithEmailSmtp.Get();
            string userName = Configurations.ToNotifyWithEmailUserName.Get();
            string password = Configurations.ToNotifyWithEmailPassword.Get();

            _ = Task.Run(() =>
            {
                _ = Notifier.SendEmail(smtpServer, userName, password, room.NickName, room.RoomUrl);
            }, token);
        }
    }
}

public sealed class RecMessage
{
    public RecMessageType Type { get; set; } = default;
    public object? Data { get; set; } = null;

    public enum RecMessageType
    {
        Unknown,
        StartStreaming,
        StopStreaming,
        StartRecording,
        StopRecording,
    }
}
