using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.ShellObjectWatcher;

[SupportedOSPlatform("Windows")]
internal class ChangeNotifyEventManager
{
    private static readonly ShellObjectChangeTypes[] _changeOrder =
    [
        ShellObjectChangeTypes.ItemCreate,
        ShellObjectChangeTypes.ItemRename,
        ShellObjectChangeTypes.ItemDelete,

        ShellObjectChangeTypes.AttributesChange,

        ShellObjectChangeTypes.DirectoryCreate,
        ShellObjectChangeTypes.DirectoryDelete,
        ShellObjectChangeTypes.DirectoryContentsUpdate,
        ShellObjectChangeTypes.DirectoryRename,

        ShellObjectChangeTypes.Update,

        ShellObjectChangeTypes.MediaInsert,
        ShellObjectChangeTypes.MediaRemove,
        ShellObjectChangeTypes.DriveAdd,
        ShellObjectChangeTypes.DriveRemove,
        ShellObjectChangeTypes.NetShare,
        ShellObjectChangeTypes.NetUnshare,

        ShellObjectChangeTypes.ServerDisconnect,
        ShellObjectChangeTypes.SystemImageUpdate,

        ShellObjectChangeTypes.AssociationChange,
        ShellObjectChangeTypes.FreeSpace,

        ShellObjectChangeTypes.DiskEventsMask,
        ShellObjectChangeTypes.GlobalEventsMask,
        ShellObjectChangeTypes.AllEventsMask
    ];

    private readonly Dictionary<ShellObjectChangeTypes, Delegate> _events = [];

    public void Register(ShellObjectChangeTypes changeType, Delegate handler)
    {
        if (!_events.TryGetValue(changeType, out var del))
        {
            _events.Add(changeType, handler);
        }
        else
        {
            del = MulticastDelegate.Combine(del, handler);
            _events[changeType] = del;
        }
    }

    public void Unregister(ShellObjectChangeTypes changeType, Delegate handler)
    {
        if (_events.TryGetValue(changeType, out var del))
        {
            del = MulticastDelegate.Remove(del, handler);
            if (del == null)
            {
                _events.Remove(changeType);
            }
            else
            {
                _events[changeType] = del;
            }
        }
    }

    public void UnregisterAll() => _events.Clear();

    public void Invoke(object sender, ShellObjectChangeTypes changeType, EventArgs args)
    {
        changeType &= ~ShellObjectChangeTypes.FromInterrupt;

        foreach (var change in _changeOrder.Where(x => (x & changeType) != 0))
        {
            if (_events.TryGetValue(change, out var del))
            {
                del.DynamicInvoke(sender, args);
            }
        }
    }

    public ShellObjectChangeTypes RegisteredTypes => _events.Keys.Aggregate<ShellObjectChangeTypes, ShellObjectChangeTypes>(
                ShellObjectChangeTypes.None,
                (accumulator, changeType) => (changeType | accumulator));
}
