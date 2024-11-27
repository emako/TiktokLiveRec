using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TiktokLiveRec.Core;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public sealed partial class TiktokSpider : ISpider
{
    public static Lazy<TiktokSpider> Instance { get; } = new(() => new TiktokSpider());

    public ISpiderResult GetResult(string url)
    {
        string? roomUrl = ParseUrl(url);
        string? htmlStr = RequestUrl(roomUrl);
        TiktokSpiderResult result = ExtractData(htmlStr);

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

        if (!userId.StartsWith('@'))
        {
            if (uri.Segments.Length >= 2)
            {
                userId = uri.Segments[^2].Trim('/');

                if (userId.StartsWith('@'))
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

        string cookie = Configurations.CookieOversea.Get();

        request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:124.0) Gecko/20100101 Firefox/124.0");
        request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
        request.AddHeader("Referer", "https://www.tiktok.com/");
        request.AddHeader("Cookie", !string.IsNullOrWhiteSpace(cookie) ? cookie : "ttwid=1%7CM-rF193sJugKuNz2RGNt-rh6pAAR9IMceUSzlDnPCNI%7C1683274418%7Cf726d4947f2fc37fecc7aeb0cdaee52892244d04efde6f8a8edd2bb168263269; tiktok_webapp_theme=light; tt_chain_token=VWkygAWDlm1cFg/k8whmOg==; passport_csrf_token=6e422c5a7991f8cec7033a8082921510; passport_csrf_token_default=6e422c5a7991f8cec7033a8082921510; d_ticket=f8c267d4af4523c97be1ccb355e9991e2ae06; odin_tt=320b5f386cdc23f347be018e588873db7f7aea4ea5d1813681c3fbc018ea025dde957b94f74146dbc0e3612426b865ccb95ec8abe4ee36cca65f15dbffec0deff7b0e69e8ea536d46e0f82a4fc37d211; cmpl_token=AgQQAPNSF-RO0rT04baWtZ0T_jUjl4fVP4PZYM2QPw; uid_tt=319b558dbba684bb1557206c92089cd113a875526a89aee30595925d804b81c7; uid_tt_ss=319b558dbba684bb1557206c92089cd113a875526a89aee30595925d804b81c7; sid_tt=ad5e736f4bedb2f6d42ccd849e706b1d; sessionid=ad5e736f4bedb2f6d42ccd849e706b1d; sessionid_ss=ad5e736f4bedb2f6d42ccd849e706b1d; store-idc=useast5; store-country-code=us; store-country-code-src=uid; tt-target-idc=useast5; tt-target-idc-sign=qXNk0bb1pDQ0FbCNF120Pl9WWMLZg9Edv5PkfyCbS4lIk5ieW5tfLP7XWROnN0mEaSlc5hg6Oji1pF-yz_3ZXnUiNMrA9wNMPvI6D9IFKKVmq555aQzwPIGHv0aQC5dNRgKo5Z5LBkgxUMWEojTKclq2_L8lBciw0IGdhFm_XyVJtbqbBKKgybGDLzK8ZyxF4Jl_cYRXaDlshZjc38JdS6wruDueRSHe7YvNbjxCnApEFUv-OwJANSPU_4rvcqpVhq3JI2VCCfw-cs_4MFIPCDOKisk5EhAo2JlHh3VF7_CLuv80FXg_7ZqQ2pJeMOog294rqxwbbQhl3ATvjQV_JsWyUsMd9zwqecpylrPvtySI2u1qfoggx1owLrrUynee1R48QlanLQnTNW_z1WpmZBgVJqgEGLwFoVOmRzJuFFNj8vIqdjM2nDSdWqX8_wX3wplohkzkPSFPfZgjzGnQX28krhgTytLt7BXYty5dpfGtsdb11WOFHM6MZ9R9uLVB; sid_guard=ad5e736f4bedb2f6d42ccd849e706b1d%7C1690990657%7C15525213%7CMon%2C+29-Jan-2024+08%3A11%3A10+GMT; sid_ucp_v1=1.0.0-KGM3YzgwYjZhODgyYWI1NjIwNTA0NjBmOWUxMGRhMjIzYTI2YjMxNDUKGAiqiJ30keKD5WQQwfCppgYYsws4AkDsBxAEGgd1c2Vhc3Q1IiBhZDVlNzM2ZjRiZWRiMmY2ZDQyY2NkODQ5ZTcwNmIxZA; ssid_ucp_v1=1.0.0-KGM3YzgwYjZhODgyYWI1NjIwNTA0NjBmOWUxMGRhMjIzYTI2YjMxNDUKGAiqiJ30keKD5WQQwfCppgYYsws4AkDsBxAEGgd1c2Vhc3Q1IiBhZDVlNzM2ZjRiZWRiMmY2ZDQyY2NkODQ5ZTcwNmIxZA; tt_csrf_token=dD0EIH8q-pe3qDQsCyyD1jLN6KizJDRjOEyk; __tea_cache_tokens_1988={%22_type_%22:%22default%22%2C%22user_unique_id%22:%227229608516049831425%22%2C%22timestamp%22:1683274422659}; ttwid=1%7CM-rF193sJugKuNz2RGNt-rh6pAAR9IMceUSzlDnPCNI%7C1694002151%7Cd89b77afc809b1a610661a9d1c2784d80ebef9efdd166f06de0d28e27f7e4efe; msToken=KfJAVZ7r9D_QVeQlYAUZzDFbc1Yx-nZz6GF33eOxgd8KlqvTg1lF9bMXW7gFV-qW4MCgUwnBIhbiwU9kdaSpgHJCk-PABsHCtTO5J3qC4oCTsrXQ1_E0XtbqiE4OVLZ_jdF1EYWgKNPT2SnwGkQ=; msToken=KfJAVZ7r9D_QVeQlYAUZzDFbc1Yx-nZz6GF33eOxgd8KlqvTg1lF9bMXW7gFV-qW4MCgUwnBIhbiwU9kdaSpgHJCk-PABsHCtTO5J3qC4oCTsrXQ1_E0XtbqiE4OVLZ_jdF1EYWgKNPT2SnwGkQ=");

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

    public static TiktokSpiderResult ExtractData(string? htmlStr)
    {
        TiktokSpiderResult result = new();

        if (string.IsNullOrWhiteSpace(htmlStr))
        {
            return result;
        }

        if (htmlStr.Contains("We regret to inform you that we have discontinued operating TikTok"))
        {
            // Your proxy node's regional network is blocked from accessing TikTok;
            // please switch to a node in another region to access.
            return result;
        }

        if (htmlStr.Contains("UNEXPECTED_EOF_WHILE_READING"))
        {
            // UNEXPECTED_EOF_WHILE_READING
            return result;
        }

        Match match = JsonRegex.Match(htmlStr);

        if (match.Success)
        {
            string jsonStr = match.Groups[1].Value;

            try
            {
                dynamic? json = JsonConvert.DeserializeObject(jsonStr);
                dynamic? liveRoom = json!["LiveRoom"]["liveRoomUserInfo"];
                dynamic? user = liveRoom["user"];

                result.UniqueId = user["uniqueId"];
                result.Nickname = user["nickname"];
                result.AvatarThumbUrl = user["avatarThumb"];
                result.IsLiveStreaming = user["status"] == "2";

                if (result.IsLiveStreaming == false)
                {
                    return result;
                }

                dynamic? streamData = liveRoom["liveRoom"]["streamData"]["pull_data"]["stream_data"];
                streamData = JsonConvert.DeserializeObject(streamData.ToString())["data"];

                result.FlvUrl = streamData["origin"]["main"]["flv"];
                result.HlsUrl = streamData["origin"]["main"]["hls"];
            }
            catch
            {
                ///
            }
        }

        return result;
    }

    [GeneratedRegex("<script id=\"SIGI_STATE\" type=\"application/json\">(.*?)</script>", RegexOptions.Singleline)]
    private static partial Regex JsonRegex { get; }
}

public sealed class TiktokSpiderResult : ISpiderResult
{
    public string? RoomUrl { get; set; }

    public bool? IsLiveStreaming { get; set; }

    public string? Nickname { get; set; }

    public string? AvatarThumbUrl { get; set; }

    public string? UniqueId { get; set; }

    public string AnchorName => $"{Nickname}-{UniqueId}";

    public string? FlvUrl { get; set; }

    public string? HlsUrl { get; set; }
}
