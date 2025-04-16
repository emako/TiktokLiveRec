using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public sealed class ShellSearchConnector : ShellSearchCollection
{
    internal ShellSearchConnector() => CoreHelpers.ThrowIfNotWin7();

    internal ShellSearchConnector(IShellItem2 shellItem)
        : this() => nativeShellItem = shellItem;
}
