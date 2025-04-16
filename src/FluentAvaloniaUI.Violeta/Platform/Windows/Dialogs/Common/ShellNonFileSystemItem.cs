using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public class ShellNonFileSystemItem : ShellObject
{
    internal ShellNonFileSystemItem(IShellItem2 shellItem) => nativeShellItem = shellItem;
}
