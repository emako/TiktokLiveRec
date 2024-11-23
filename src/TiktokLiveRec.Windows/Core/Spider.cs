namespace TiktokLiveRec.Core;

internal static class Spider
{
    public static ISpiderResult? GetResult(string url)
    {
        // Only support following spider now.

        if (url.Contains("douyin"))
        {
            return DouyinSpider.Instance.Value.GetResult(url);
        }
        else if (url.Contains("tiktok"))
        {
            return TiktokSpider.Instance.Value.GetResult(url);
        }

        return null;
    }
}

public interface ISpider
{
    public ISpiderResult GetResult(string url);
}

public interface ISpiderResult
{
    public string? RoomUrl { get; set; }

    public bool? IsLiveStreaming { get; set; }

    public string? Nickname { get; set; }

    public string? FlvUrl { get; set; }

    public string? HlsUrl { get; set; }
}
