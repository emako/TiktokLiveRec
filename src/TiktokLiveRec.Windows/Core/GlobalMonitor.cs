using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Fischless.Configuration;
using MediaInfoLib;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Web;
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
                Room[] rooms = Configurations.Rooms.Get();

                // Check Global Settings
                bool isGlobalToNotify = Configurations.IsToNotify.Get();
                bool isGlobalToRecord = Configurations.IsToRecord.Get();

                foreach (Room room in rooms)
                {
                    if (TryGetRoomStatus(room) is RoomStatus roomStatus)
                    {
                        // Check Room Settings
                        bool isRoomToNotify = room.IsToNotify;
                        bool isRoomToRecord = room.IsToRecord;

                        if ((isGlobalToNotify || isGlobalToRecord) && (isRoomToNotify || isRoomToRecord))
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
                            if (isRoomToRecord && isGlobalToRecord)
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
                            else
                            {
                                if (roomStatus.RecordStatus != RecordStatus.Recording)
                                {
                                    roomStatus.RecordStatus = RecordStatus.Disabled;
                                }
                            }

                            // Start Broadcast Notification
                            if (isRoomToNotify && isGlobalToNotify)
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
                            else
                            {
                                roomStatus.StreamStatus = StreamStatus.Disabled;
                            }
                        }
                        else
                        {
                            if (roomStatus.RecordStatus != RecordStatus.Recording)
                            {
                                roomStatus.RecordStatus = RecordStatus.Disabled;
                            }
                            roomStatus.StreamStatus = StreamStatus.Disabled;
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
            Notifier.AddNoticeWithButton("LiveNotification".Tr(), room.NickName, [
                new ToastContentButtonOption()
                    {
                        Content = "GotoLiveRoom".Tr(),
                        Arguments = [("RoomUrl", room.RoomUrl)],
                        ActivationType = ToastActivationType.Background,
                    },
                    new ToastContentButtonOption()
                    {
                        Content = "ButtonOfClose".Tr(),
                        ActivationType = ToastActivationType.Foreground,
                    },
                ]);
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
