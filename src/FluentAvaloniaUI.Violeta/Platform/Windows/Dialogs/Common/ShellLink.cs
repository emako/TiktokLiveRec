using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

#pragma warning disable CS8618

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public class ShellLink : ShellObject
{
    private string _internalPath;

    internal ShellLink(IShellItem2 shellItem) => nativeShellItem = shellItem;

    public virtual string Path
    {
        get
        {
            if (_internalPath == null && NativeShellItem != null)
            {
                _internalPath = base.ParsingName;
            }
            return _internalPath!;
        }
        protected set => _internalPath = value;
    }
}
