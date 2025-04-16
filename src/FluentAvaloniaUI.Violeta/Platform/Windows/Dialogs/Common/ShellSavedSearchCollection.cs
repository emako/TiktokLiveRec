using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public class ShellSavedSearchCollection : ShellSearchCollection
{
    internal ShellSavedSearchCollection(IShellItem2 shellItem)
        : base(shellItem) => CoreHelpers.ThrowIfNotVista();
}
