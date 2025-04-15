using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellSearchCollection : ShellContainer
{
    internal ShellSearchCollection()
    {
    }

    internal ShellSearchCollection(IShellItem2 shellItem) : base(shellItem)
    {
    }
}
