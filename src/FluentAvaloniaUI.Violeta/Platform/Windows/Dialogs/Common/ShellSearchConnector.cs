using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public sealed class ShellSearchConnector : ShellSearchCollection
{
    internal ShellSearchConnector() => CoreHelpers.ThrowIfNotWin7();

    internal ShellSearchConnector(IShellItem2 shellItem)
        : this() => nativeShellItem = shellItem;
}
