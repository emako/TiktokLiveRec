using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellNonFileSystemItem : ShellObject
{
    internal ShellNonFileSystemItem(IShellItem2 shellItem) => nativeShellItem = shellItem;
}
