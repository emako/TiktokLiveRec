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

        using (LoadingWindow.ShowAsync())
        {
            try
            {
                ISpiderResult? spider = Spider.GetResult(Url);

                if (string.IsNullOrWhiteSpace(spider?.Nickname))
                {
                    e.Cancel = true;
                    Toast.Error("GetRoomInfoError".Tr());
                    return;
                }

                if (Configurations.Rooms.Get().Any(room => room.RoomUrl == spider.RoomUrl))
                {
                    e.Cancel = true;
                    Toast.Warning("AddRoomErrorDuplicated".Tr(spider.Nickname));
                    return;
                }

                NickName = spider.Nickname;
                RoomUrl = spider.RoomUrl;

                Toast.Success("AddRoomSucc".Tr(NickName));
            }
            catch
            {
                e.Cancel = true;
                Toast.Error("ErrorRoomUrl".Tr());
            }
        }
    }
}
