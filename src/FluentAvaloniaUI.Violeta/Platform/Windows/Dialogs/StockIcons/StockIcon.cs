using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.StockIcons;
using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.StockIcons;

public class StockIcon : IDisposable
{
    private StockIconIdentifier identifier = StockIconIdentifier.Application;
    private StockIconSize currentSize = StockIconSize.Large;
    private bool linkOverlay;
    private bool selected;
    private bool invalidateIcon = true;
    private nint hIcon = 0;

    public StockIcon(StockIconIdentifier id)
    {
        identifier = id;
        invalidateIcon = true;
    }

    public StockIcon(StockIconIdentifier id, StockIconSize size, bool isLinkOverlay, bool isSelected)
    {
        identifier = id;
        linkOverlay = isLinkOverlay;
        selected = isSelected;
        currentSize = size;
        invalidateIcon = true;
    }

    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            invalidateIcon = true;
        }
    }

    public bool LinkOverlay
    {
        get => linkOverlay;
        set
        {
            linkOverlay = value;
            invalidateIcon = true;
        }
    }

    public StockIconSize CurrentSize
    {
        get => currentSize;
        set
        {
            currentSize = value;
            invalidateIcon = true;
        }
    }

    public StockIconIdentifier Identifier
    {
        get => identifier;
        set
        {
            identifier = value;
            invalidateIcon = true;
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }

        if (hIcon != 0)
            _ = User32.DestroyIcon(hIcon);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~StockIcon()
    {
        Dispose(false);
    }
}
