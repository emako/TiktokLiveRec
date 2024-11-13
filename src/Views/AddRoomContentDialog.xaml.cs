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
            Toast.Warning("EnterRoomUrl".Tr());
            e.Cancel = true;
            return;
        }

        try
        {
            ISpiderResult? spider = Spider.GetResult(Url);

            if (string.IsNullOrWhiteSpace(spider?.Nickname))
            {
                Toast.Error("GetRoomInfoError".Tr());
                e.Cancel = true;
                return;
            }

            NickName = spider.Nickname;
            RoomUrl = spider.RoomUrl;

            Toast.Success("AddRoomSucc".Tr(NickName));
        }
        catch
        {
            Toast.Error("ErrorRoomUrl".Tr());
            e.Cancel = true;
        }
    }
}
