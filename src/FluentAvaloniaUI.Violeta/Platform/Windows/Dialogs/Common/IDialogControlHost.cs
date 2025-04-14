namespace MicaSetup.Shell.Dialogs;

public interface IDialogControlHost
{
    public void ApplyCollectionChanged();

    public void ApplyControlPropertyChange(string propertyName, DialogControl control);

    public bool IsCollectionChangeAllowed();

    public bool IsControlPropertyChangeAllowed(string propertyName, DialogControl control);
}
