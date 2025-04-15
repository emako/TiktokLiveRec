namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

public abstract class CommonFileDialogProminentControl : CommonFileDialogControl
{
    private bool isProminent;

    protected CommonFileDialogProminentControl()
    {
    }

    protected CommonFileDialogProminentControl(string text) : base(text)
    {
    }

    protected CommonFileDialogProminentControl(string name, string text) : base(name, text)
    {
    }

    public bool IsProminent
    {
        get => isProminent;
        set => isProminent = value;
    }
}
