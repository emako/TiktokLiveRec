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

    public string? RoomUrl = null;

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
            ISpiderResult? spider = Spider.GetResult(Url);

            if (string.IsNullOrWhiteSpace(spider?.Nickname))
            {
                Toast.Error("获取直播间信息失败");
                e.Cancel = true;
                return;
            }

            NickName = spider.Nickname;
            RoomUrl = spider.RoomUrl;

            Toast.Success($"成功添加 {NickName} 直播间");
        }
        catch
        {
            Toast.Error("错误的直播间链接");
            e.Cancel = true;
        }
    }
}
