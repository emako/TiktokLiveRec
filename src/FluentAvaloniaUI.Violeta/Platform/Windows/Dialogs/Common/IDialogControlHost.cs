using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public interface IDialogControlHost
{
    public void ApplyCollectionChanged();

    public void ApplyControlPropertyChange(string propertyName, DialogControl control);

    public bool IsCollectionChangeAllowed();

    public bool IsControlPropertyChangeAllowed(string propertyName, DialogControl control);
}
