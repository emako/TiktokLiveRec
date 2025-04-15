namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

internal interface ICommonFileDialogIndexedControls
{
    public event EventHandler SelectedIndexChanged;

    public int SelectedIndex { get; set; }

    public void RaiseSelectedIndexChangedEvent();
}
