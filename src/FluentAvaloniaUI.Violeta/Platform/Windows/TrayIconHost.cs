using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace FluentAvalonia.UI.Violeta.Platform.Windows;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public class TrayIconHost
{
    private const int ID_TRAYICON = 1000;

    private readonly nint hWnd = nint.Zero;
    private readonly User32.WndProcDelegate wndProcDelegate = null!;
    private Shell32.NotifyIconData notifyIconData = default;

    public string ToolTipText
    {
        get => notifyIconData.szTip;
        set
        {
            notifyIconData.szTip = value;
            notifyIconData.uFlags |= (int)Shell32.NotifyIconFlags.NIF_TIP;
            _ = Shell32.Shell_NotifyIcon((int)Shell32.NOTIFY_COMMAND.NIM_MODIFY, ref notifyIconData);
        }
    }

    public nint Icon
    {
        get => notifyIconData.hIcon;
        set
        {
            if (notifyIconData.hIcon != nint.Zero)
                _ = User32.DestroyIcon(notifyIconData.hIcon);
            notifyIconData.hIcon = User32.CopyIcon(value);
            notifyIconData.uFlags |= (int)Shell32.NotifyIconFlags.NIF_ICON;
            _ = Shell32.Shell_NotifyIcon((int)Shell32.NOTIFY_COMMAND.NIM_MODIFY, ref notifyIconData);
        }
    }

    public bool IsVisible
    {
        get => notifyIconData.dwState != (uint)Shell32.NotifyIconState.NIS_HIDDEN;
        set
        {
            notifyIconData.dwState = value ? 0 : (uint)Shell32.NotifyIconState.NIS_HIDDEN;
            notifyIconData.dwStateMask = (uint)(Shell32.NotifyIconState.NIS_HIDDEN | Shell32.NotifyIconState.NIS_SHAREDICON);
            notifyIconData.uFlags |= (int)Shell32.NotifyIconFlags.NIF_STATE;
            _ = Shell32.Shell_NotifyIcon((int)Shell32.NOTIFY_COMMAND.NIM_MODIFY, ref notifyIconData);
        }
    }

    public TrayMenu Menu { get; set; } = null!;

    public event EventHandler<EventArgs>? Click;

    public event EventHandler<EventArgs>? RightDown;

    public event EventHandler<EventArgs>? RightClick;

    public event EventHandler<EventArgs>? RightDoubleClick;

    public event EventHandler<EventArgs>? LeftDown;

    public event EventHandler<EventArgs>? LeftClick;

    public event EventHandler<EventArgs>? LeftDoubleClick;

    public event EventHandler<EventArgs>? MiddleDown;

    public event EventHandler<EventArgs>? MiddleClick;

    public event EventHandler<EventArgs>? MiddleDoubleClick;

    public TrayIconHost()
    {
        wndProcDelegate = new User32.WndProcDelegate(WndProc);

        User32.WNDCLASS wc = new()
        {
            lpszClassName = "TrayIconHostWindowClass",
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate)
        };
        User32.RegisterClass(ref wc);

        hWnd = User32.CreateWindowEx(0, "TrayIconHostWindowClass", "TrayIconHostWindow", 0, 0, 0, 0, 0,
            nint.Zero, nint.Zero, nint.Zero, nint.Zero);

        notifyIconData = new Shell32.NotifyIconData()
        {
            cbSize = Marshal.SizeOf<Shell32.NotifyIconData>(),
            hWnd = hWnd,
            uID = ID_TRAYICON,
            uFlags = (int)(Shell32.NotifyIconFlags.NIF_ICON | Shell32.NotifyIconFlags.NIF_MESSAGE | Shell32.NotifyIconFlags.NIF_TIP),
            uCallbackMessage = (int)User32.WindowMessage.WM_TRAYICON,
            hIcon = nint.Zero,
            szTip = null!,
        };

        _ = Shell32.Shell_NotifyIcon((int)Shell32.NOTIFY_COMMAND.NIM_ADD, ref notifyIconData);
    }

    protected virtual nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        if (msg == (uint)User32.WindowMessage.WM_TRAYICON)
        {
            if (wParam.ToInt32() == ID_TRAYICON)
            {
                User32.WindowMessage mouseMsg = (User32.WindowMessage)lParam.ToInt32();

                switch (mouseMsg)
                {
                    case User32.WindowMessage.WM_QUERYENDSESSION:
                    case User32.WindowMessage.WM_ENDSESSION:
                        _ = Shell32.Shell_NotifyIcon((int)Shell32.NOTIFY_COMMAND.NIM_DELETE, ref notifyIconData);
                        break;

                    case User32.WindowMessage.WM_LBUTTONDOWN:
                        LeftDown?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_LBUTTONUP:
                        LeftClick?.Invoke(this, EventArgs.Empty);
                        Click?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_LBUTTONDBLCLK:
                        LeftDoubleClick?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_RBUTTONDOWN:
                        RightDown?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_RBUTTONUP:
                        RightClick?.Invoke(this, EventArgs.Empty);
                        ShowContextMenu();
                        break;

                    case User32.WindowMessage.WM_RBUTTONDBLCLK:
                        RightDoubleClick?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_MBUTTONDOWN:
                        MiddleDown?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_MBUTTONUP:
                        MiddleClick?.Invoke(this, EventArgs.Empty);
                        Click?.Invoke(this, EventArgs.Empty);
                        break;

                    case User32.WindowMessage.WM_MBUTTONDBLCLK:
                        MiddleDoubleClick?.Invoke(this, EventArgs.Empty);
                        break;
                }
            }
        }
        return User32.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    public virtual void ShowContextMenu()
    {
        Menu?.Open(hWnd);
    }
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public class TrayMenu : AvaloniaObject, IEnumerable<ITrayMenuItemBase>, IList<ITrayMenuItemBase>
{
    public static readonly DirectProperty<TrayMenu, TrayMenuItem?> ParentProperty =
        AvaloniaProperty.RegisterDirect<TrayMenu, TrayMenuItem?>(nameof(Parent), o => o.Parent);

    private TrayMenuItem? _parent;

    public TrayMenuItem? Parent
    {
        get => _parent;
        internal set => SetAndRaise(ParentProperty, ref _parent, value);
    }

    private readonly AvaloniaList<ITrayMenuItemBase> _items = new() { ResetBehavior = ResetBehavior.Remove };

    [Content]
    public IList<ITrayMenuItemBase> Items => _items;

    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public ITrayMenuItemBase this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public event EventHandler<EventArgs>? Opening;

    public event EventHandler<EventArgs>? Closed;

    public IEnumerator<ITrayMenuItemBase> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(ITrayMenuItemBase item) => _items.IndexOf(item);

    public void Insert(int index, ITrayMenuItemBase item) => _items.Insert(index, item);

    public void RemoveAt(int index) => _items.RemoveAt(index);

    public void Add(ITrayMenuItemBase item) => _items.Add(item);

    public void Clear() => _items.Clear();

    public bool Contains(ITrayMenuItemBase item) => _items.Contains(item);

    public void CopyTo(ITrayMenuItemBase[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public bool Remove(ITrayMenuItemBase item) => _items.Contains(item);

    public void Open(nint hWnd)
    {
        if (_items.Count == 0) return;

        nint hMenu = User32.CreatePopupMenu();
        if (hMenu == nint.Zero) return;

        Opening?.Invoke(this, EventArgs.Empty);

        Dictionary<uint, ITrayMenuItemBase> idToItem = [];
        uint currentId = 1000;

        foreach (ITrayMenuItemBase item in _items)
        {
            if (!item.IsVisible) continue;

            if (item.Header == "-" || item is TraySeparator)
            {
                _ = User32.AppendMenu(hMenu, (uint)User32.MenuFlags.MF_SEPARATOR, 0, string.Empty);
            }
            else
            {
                var flags = User32.MenuFlags.MF_STRING;

                if (!item.IsEnabled)
                    flags |= User32.MenuFlags.MF_DISABLED | User32.MenuFlags.MF_GRAYED;

                if (item.IsChecked)
                    flags |= User32.MenuFlags.MF_CHECKED;

                _ = User32.AppendMenu(hMenu, (uint)flags, currentId, item.Header!);
                idToItem[currentId] = item;
                currentId++;
            }
        }

        _ = User32.GetCursorPos(out User32.POINT pt);

        User32.TrackPopupMenuFlags flag =
            User32.TrackPopupMenuFlags.TPM_RETURNCMD |
            User32.TrackPopupMenuFlags.TPM_VERTICAL |
            User32.TrackPopupMenuFlags.TPM_LEFTALIGN;

        _ = User32.SetForegroundWindow(hWnd);
        uint selected = User32.TrackPopupMenuEx(hMenu, (uint)flag, pt.X, pt.Y, hWnd, nint.Zero);
        _ = User32.PostMessage(hWnd, 0, nint.Zero, nint.Zero);

        if (selected != 0 && idToItem.TryGetValue(selected, out ITrayMenuItemBase? clickedItem))
        {
            clickedItem.Command?.Execute(clickedItem.CommandParameter);
        }

        User32.DestroyMenu(hMenu);

        Closed?.Invoke(this, EventArgs.Empty);
    }
}

public interface ITrayMenuItemBase
{
    public TrayMenu? Menu { get; set; }

    public Bitmap? Icon { get; set; }

    public string? Header { get; set; }

    public bool IsVisible { get; set; }

    public bool IsChecked { get; set; }

    public bool IsEnabled { get; set; }

    public ICommand? Command { get; set; }

    public object? CommandParameter { get; set; }
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public sealed class TraySeparator : AvaloniaObject, ITrayMenuItemBase
{
    public TrayMenu? Menu
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public Bitmap? Icon
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public string? Header
    {
        get => "-";
        set => throw new NotImplementedException();
    }

    public bool IsVisible
    {
        get => true;
        set => throw new NotImplementedException();
    }

    public bool IsChecked
    {
        get => false;
        set => throw new NotImplementedException();
    }

    public bool IsEnabled
    {
        get => false;
        set => throw new NotImplementedException();
    }

    public ICommand? Command
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public object? CommandParameter
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public class TrayMenuItem : AvaloniaObject, ITrayMenuItemBase
{
    public static readonly StyledProperty<TrayMenu?> MenuProperty =
        AvaloniaProperty.Register<TrayMenuItem, TrayMenu>(nameof(Menu), null!, inherits: false, BindingMode.OneWay, null, CoerceMenu)!;

    private static TrayMenu CoerceMenu(AvaloniaObject sender, TrayMenu? value)
    {
        if (value != null)
        {
            throw new InvalidOperationException("TrayMenu already has a parent");
        }

        return value!;
    }

    public static readonly StyledProperty<Bitmap?> IconProperty =
        AvaloniaProperty.Register<TrayMenuItem, Bitmap>(nameof(Icon))!;

    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<TrayMenuItem, string>(nameof(Header))!;

    public static readonly StyledProperty<bool> IsCheckedProperty =
        MenuItem.IsCheckedProperty.AddOwner<TrayMenuItem>();

    public static readonly StyledProperty<ICommand?> CommandProperty =
        MenuItem.CommandProperty.AddOwner<TrayMenuItem>(
            new StyledPropertyMetadata<ICommand>(default, BindingMode.Default, null, enableDataValidation: true)!);

    public static readonly StyledProperty<object?> CommandParameterProperty =
        MenuItem.CommandParameterProperty.AddOwner<TrayMenuItem>();

    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<TrayMenuItem, bool>(nameof(IsEnabled), defaultValue: true);

    public static readonly StyledProperty<bool> IsVisibleProperty =
        Visual.IsVisibleProperty.AddOwner<TrayMenuItem>();

    [Content]
    public TrayMenu? Menu
    {
        get => GetValue(MenuProperty);
        set => SetValue(MenuProperty, value);
    }

    public Bitmap? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public bool IsVisible
    {
        get => GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}
