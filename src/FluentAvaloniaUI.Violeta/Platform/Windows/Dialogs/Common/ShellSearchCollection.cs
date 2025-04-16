using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public class ShellSearchCollection : ShellContainer
{
    internal ShellSearchCollection()
    {
    }

    internal ShellSearchCollection(IShellItem2 shellItem) : base(shellItem)
    {
    }
}
