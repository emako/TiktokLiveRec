using RestSharp;
using System.Reactive.Joins;
using System.Text.RegularExpressions;

namespace TiktokLiveRec.Core;

public sealed partial class TiktokSpider : ISpider
{
    public static Lazy<TiktokSpider> Instance { get; } = new(() => new TiktokSpider());

    public ISpiderResult GetResult(string url)
    {
        string? roomUrl = ParseUrl(url);
        string? htmlStr = RequestUrl(roomUrl);

        TiktokSpiderResult result = new();

        result.RoomUrl = roomUrl;
        return result;
    }

    public string? ParseUrl(string url)
    {
        // Supported two case URLs:
        // https://www.tiktok.com/@xxx/live
        Uri uri = new(url);

        if (uri.Host != "www.tiktok.com")
        {
            return null;
        }

        string userId = uri.Segments.Last();

        if (!userId.StartsWith("@"))
        {
            if (uri.Segments.Length >= 2)
            {
                userId = uri.Segments[^2].Trim('/');

                if (userId.StartsWith("@"))
                {
                    string roomUrl = $"https://www.tiktok.com/{userId}/live";
                    return roomUrl;
                }
            }
        }

        return null;
    }

    private string? RequestUrl(string? url)
    {
        if (url == null)
        {
            return null;
        }

        RestClientOptions options = new()
        {
            BaseUrl = new Uri(url),
        };

        RestClient client = new(options);

        RestRequest request = new()
        {
            Method = Method.Get,
        };

        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/115.0");
        request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
        request.AddHeader("Referer", "https://www.tiktok.com/");

        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            string? htmlStr = response.Content;

            Console.WriteLine(htmlStr);
            return htmlStr;
        }
        else
        {
            Console.WriteLine($"{response.ErrorMessage}");
            return null!;
        }
    }
}

public sealed class TiktokSpiderResult : ISpiderResult
{
    public string? RoomUrl { get; set; }

    public bool? IsLiveStreaming { get; set; }

    public string? Nickname { get; set; }

    public string? HlsUrl { get; set; }
}
