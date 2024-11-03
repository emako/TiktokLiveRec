using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using Windows.System;

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
                if (Configurations.IsToNotify.Get() || Configurations.IsToRecord.Get())
                {
                    Room[] rooms = Configurations.Rooms.Get();

                    foreach (Room room in rooms)
                    {
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
                                    Notifier.AddNoticeWithButton("开播通知", room.NickName, "进入直播间", [("RoomUrl", room.RoomUrl)]);
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
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}

public sealed class RecMessage
{
    public RecMessageType Type { get; set; } = default;

    public enum RecMessageType
    {
        Unknown,
        StartStreaming,
        StopStreaming,
        StartRecording,
        StopRecording,
    }
}
