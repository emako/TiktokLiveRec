using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellNonFileSystemFolder : ShellFolder
{
    internal ShellNonFileSystemFolder()
    {
    }

    internal ShellNonFileSystemFolder(IShellItem2 shellItem) => nativeShellItem = shellItem;
}
