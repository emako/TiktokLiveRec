using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.UI.Controls;
using TiktokLiveRec.Core;

namespace TiktokLiveRec.Views;

public sealed partial class AddRoomContentDialogContent : UserControl
{
    public ContentDialogResult Result { get; set; } = ContentDialogResult.None;

    public string? Url
    {
        get => ViewModel.Url;
        private set => ViewModel.Url = value;
    }

    public bool IsForcedAdd
    {
        get => ViewModel.IsForcedAdd;
        private set => ViewModel.IsForcedAdd = value;
    }

    public string? NickName
    {
        get => ViewModel.NickName;
        private set => ViewModel.NickName = value;
    }

    public string? RoomUrl
    {
        get => ViewModel.RoomUrl;
        private set => ViewModel.RoomUrl = value;
    }

    public AddRoomContentDialogViewModel ViewModel { get; }

    public AddRoomContentDialogContent()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == ParentProperty)
        {
            if (Parent is ContentDialog dialog)
            {
                dialog.Title = "AddRoom".Tr();
                dialog.PrimaryButtonText = "ButtonOfAdd".Tr();
                dialog.CloseButtonText = "ButtonOfClose".Tr();
                dialog.PrimaryButtonClick += OnPrimaryButtonClick;

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow is Window window)
                    {
                        _ = dialog.UseAsTitleBarForWindowFrame(window, true)
                            .UseAsTitleBarForWindowSystemMenu(window);
                    }
                }
            }
        }

        base.OnPropertyChanged(change);
    }

    private async void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            Toast.Warning("EnterRoomUrl".Tr());
            e.Cancel = true;
            return;
        }

        if (IsForcedAdd)
        {
            string? roomUrl = Spider.ParseUrl(Url);

            if (roomUrl != null)
            {
                if (Configurations.Rooms.Get().Any(room => room.RoomUrl == roomUrl))
                {
                    e.Cancel = true;
                    Toast.Warning("AddRoomErrorDuplicated".Tr(roomUrl));
                    return;
                }

                NickName = roomUrl;
                RoomUrl = roomUrl;

                Result = ContentDialogResult.Primary;
                Toast.Success("AddRoomSucc".Tr(RoomUrl));
            }
            else
            {
                Toast.Error("ErrorRoomUrl".Tr());
            }
        }
        else
        {
            e.Cancel = true;

            using (LoadingWindow.ShowAsync())
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        ISpiderResult? spider = Spider.GetResult(Url);

                        if (string.IsNullOrWhiteSpace(spider?.Nickname))
                        {
                            e.Cancel = true;
                            await Dispatcher.UIThread.InvokeAsync(() => Toast.Error("GetRoomInfoError".Tr()));
                            return;
                        }

                        if (Configurations.Rooms.Get().Any(room => room.RoomUrl == spider.RoomUrl))
                        {
                            e.Cancel = true;
                            await Dispatcher.UIThread.InvokeAsync(() => Toast.Warning("AddRoomErrorDuplicated".Tr(spider.Nickname)));
                            return;
                        }

                        NickName = spider.Nickname;
                        RoomUrl = spider.RoomUrl;

                        await Dispatcher.UIThread.InvokeAsync(() => Toast.Success("AddRoomSucc".Tr(NickName)));
                        e.Cancel = false;
                    }
                    catch
                    {
                        e.Cancel = true;
                        await Dispatcher.UIThread.InvokeAsync(() => Toast.Error("ErrorRoomUrl".Tr()));
                    }
                });

                if (!e.Cancel)
                {
                    if (Parent is ContentDialog dialog)
                    {
                        Result = ContentDialogResult.Primary;
                        dialog.Hide();
                    }
                }
            }
        }
    }
}

public sealed partial class AddRoomContentDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string? url = null;

    [ObservableProperty]
    private bool isForcedAdd = false;

    [ObservableProperty]
    private string? nickName = null;

    public string? RoomUrl = null;
}
