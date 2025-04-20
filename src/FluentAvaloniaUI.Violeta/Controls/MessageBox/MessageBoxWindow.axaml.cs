namespace FluentAvalonia.UI.Controls;

public partial class MessageBoxWindow : FluentWindow
{
    public MessageBoxStandardViewModel ViewModel { get; }

    public MessageBoxResult Result => ViewModel.Result;

    public MessageBoxWindow() : this(new())
    {
    }

    public MessageBoxWindow(MessageBoxStandardParams @params)
    {
        DataContext = ViewModel = new MessageBoxStandardViewModel(@params);
        ViewModel.CloseRequested += OnCloseRequested;

        InitializeComponent();
        ShowInTaskbar = false;
        CanResize = false;
    }

    protected virtual void OnCloseRequested(MessageBoxResult e)
    {
        Close(e);
    }

    public Task Copy()
    {
        var clipboard = GetTopLevel(this)?.Clipboard;
        var text = ContentTextBox.SelectedText;
        if (string.IsNullOrEmpty(text))
        {
            text = ViewModel?.ContentMessage;
        }
        return clipboard?.SetTextAsync(text)!;
    }
}
