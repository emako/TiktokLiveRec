using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Fischless.Configuration;
using MediaInfoLib;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Web;
using TiktokLiveRec.Models;
using TiktokLiveRec.Threading;
using Windows.System;
using Wpf.Ui.Violeta.Resources;

namespace TiktokLiveRec.Core;

internal static class GlobalMonitor
{
    /// <summary>
    /// ConcurrentDictionary{RoomUrl: string, RoomStatus: RoomStatus>}
    /// </summary>
    public static ConcurrentDictionary<string, RoomStatus> RoomStatus { get; } = new();

    public static PeriodicWait RoutinePeriodicWait = new(TimeSpan.FromMilliseconds(int.Max(Configurations.RoutineInterval.Get(), 500)), TimeSpan.Zero);

    public static CancellationTokenSource? TokenSource { get; private set; } = null;

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
                else if (parsedArgs["OffRemindTheCloseToTrayHint"] != null)
                {
                    try
                    {
                        Configurations.IsOffRemindCloseToTray.Set(true);
                        ConfigurationManager.Save();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        });
    }

    public static void Start(CancellationTokenSource? tokenSource = null)
    {
        TokenSource = tokenSource ?? new CancellationTokenSource();

        _ = Task.Factory.StartNew(async () => await StartAsync(TokenSource.Token), TaskCreationOptions.LongRunning);
    }

    public static void Stop()
    {
        TokenSource?.Cancel();
    }

    public static async Task StartAsync(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            // Delay Routine Interval
            _ = await RoutinePeriodicWait.WaitForNextTickAsync(token);

            // Routine Can't be stopped from throwables
            try
            {
                // Check Global Settings
                if (Configurations.IsToNotify.Get() || Configurations.IsToRecord.Get())
                {
                    Room[] rooms = Configurations.Rooms.Get();

                    foreach (Room room in rooms)
                    {
                        if (TryGetRoomStatus(room) is RoomStatus roomStatus)
                        {
                            // Check Room Settings
                            if (room.IsToNotify || room.IsToRecord)
                            {
                                // Spider Room Status
                                ISpiderResult? spiderResult = Spider.GetResult(room.RoomUrl);

                                if (spiderResult == null)
                                {
                                    // Not supported streaming live or error
                                    continue;
                                }

                                StreamStatus prevStreamStatus = roomStatus.StreamStatus;

                                // Update Room Status
                                roomStatus.HlsUrl = spiderResult.HlsUrl!;
                                roomStatus.StreamStatus = spiderResult.IsLiveStreaming switch
                                {
                                    true => StreamStatus.Streaming,
                                    false => StreamStatus.NotStreaming,
                                    null or _ => roomStatus.StreamStatus,
                                };

                                // Start Streaming Recording
                                if (room.IsToRecord)
                                {
                                    if (spiderResult.IsLiveStreaming ?? false)
                                    {
                                        _ = roomStatus.Recorder.Start(new RecorderStartInfo()
                                        {
                                            NickName = room.NickName,
                                            HlsUrl = roomStatus.HlsUrl,
                                        });
                                    }
                                }

                                // Start Broadcast Notification
                                if (room.IsToNotify)
                                {
                                    // Only to notify when first detected
                                    if (prevStreamStatus != StreamStatus.Streaming)
                                    {
                                        if (spiderResult.IsLiveStreaming ?? false)
                                        {
                                            await Notify(room, token);
                                        }
                                    }
                                }

                                // ?
                                _ = WeakReferenceMessenger.Default.Send(new RecMessage()
                                {
                                    Type = RecMessageType.Default,
                                });
                            }
                            else
                            {
                                // Update Room Status
                                roomStatus.StreamStatus = StreamStatus.Disabled;
                            }
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

    /// <summary>
    /// Get Room Status
    /// </summary>
    private static RoomStatus? TryGetRoomStatus(Room room)
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
            });
        }

        if (RoomStatus.TryGetValue(room.RoomUrl, out RoomStatus? roomStatus))
        {
            ///
        }

        return roomStatus;
    }

    /// <summary>
    /// Notification Runnable
    /// </summary>
    private static async Task Notify(Room room, CancellationToken token = default)
    {
        if (Configurations.IsToNotifyWithSystem.Get())
        {
            Notifier.AddNoticeWithButton("LiveNotification".Tr(), room.NickName, "GotoLiveRoom".Tr(), [("RoomUrl", room.RoomUrl)]);
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

        if (Configurations.IsToNotifyGotoRoomUrl.Get())
        {
            _ = await Launcher.LaunchUriAsync(new Uri(room.RoomUrl));

            if (Configurations.IsToNotifyGotoRoomUrlAndMute.Get())
            {
                SystemVolume.SetMasterVolumeMute(true);
            }
        }
    }
}
