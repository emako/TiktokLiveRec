using System.ComponentModel;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

public class CommonFileDialogFolderChangeEventArgs : CancelEventArgs
{
    public CommonFileDialogFolderChangeEventArgs(string folder) => Folder = folder;

    public string Folder { get; set; }
}
