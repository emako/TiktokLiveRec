using System.ComponentModel;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogFolderChangeEventArgs(string folder) : CancelEventArgs
{
    public string Folder { get; set; } = folder;
}
