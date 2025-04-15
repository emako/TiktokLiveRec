using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellSavedSearchCollection : ShellSearchCollection
{
    internal ShellSavedSearchCollection(IShellItem2 shellItem)
        : base(shellItem) => CoreHelpers.ThrowIfNotVista();
}
