using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace TiktokLiveRec.Core;

internal static class GlobalMonitor
{
    public static ConcurrentDictionary<string, RoomStatus> RoomStatus { get; } = new();

    public static async Task StartAsync(CancellationToken token = default)
    {
        using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(500));

        while (await timer.WaitForNextTickAsync(token).ConfigureAwait(false))
        {
            try
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
                                Notifier.Notify(room.NickName, "开播通知", room.RoomUrl);
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
