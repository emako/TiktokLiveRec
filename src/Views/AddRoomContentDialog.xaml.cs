using CommunityToolkit.Mvvm.ComponentModel;
using TiktokLiveRec.Core;
using Wpf.Ui.Violeta.Controls;

namespace TiktokLiveRec.Views;

[ObservableObject]
public sealed partial class AddRoomContentDialog : ContentDialog
{
    [ObservableProperty]
    private string? url = null;

    [ObservableProperty]
    private string? nickName = null;

    public AddRoomContentDialog()
    {
        DataContext = this;
        InitializeComponent();
    }

    private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            Toast.Warning("请输入直播间链接");
            e.Cancel = true;
            return;
        }

        try
        {
            // Supported two case URLs:
            // https://live.douyin.com/xxx?x=x
            // https://www.douyin.com/root/live/xxx?x=x
            Uri uri = new(Url);

            if (uri.Host != "live.douyin.com" && uri.Host != "www.douyin.com")
            {
                Toast.Warning("目前仅支持国区抖音直播间链接");
                e.Cancel = true;
                return;
            }

            string roomId = uri.Segments.Last();
            string roomUrl = $"https://live.douyin.com/{roomId}";

            SpiderResult spider = Spider.GetResult(roomUrl);

            if (string.IsNullOrWhiteSpace(spider.Nickname))
            {
                Toast.Error("获取直播间信息失败");
                e.Cancel = true;
                return;
            }

            NickName = spider.Nickname;

            Toast.Success($"成功添加{NickName}直播间");
        }
        catch
        {
            Toast.Error("错误的直播间链接");
            e.Cancel = true;
        }
    }
}
