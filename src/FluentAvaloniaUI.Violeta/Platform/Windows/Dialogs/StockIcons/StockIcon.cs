using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.StockIcons;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Style", "IDE0044:Add readonly modifier")]
public class StockIcon : IDisposable
{
    private StockIconIdentifier identifier = StockIconIdentifier.Application;
    private StockIconSize currentSize = StockIconSize.Large;
    private bool linkOverlay;
    private bool selected;
    private nint hIcon = 0;

    public StockIcon(StockIconIdentifier id)
    {
        identifier = id;
    }

    public StockIcon(StockIconIdentifier id, StockIconSize size, bool isLinkOverlay, bool isSelected)
    {
        identifier = id;
        linkOverlay = isLinkOverlay;
        selected = isSelected;
        currentSize = size;
    }

    public bool Selected
    {
        get => selected;
        set => selected = value;
    }

    public bool LinkOverlay
    {
        get => linkOverlay;
        set => linkOverlay = value;
    }

    public StockIconSize CurrentSize
    {
        get => currentSize;
        set => currentSize = value;
    }

    public StockIconIdentifier Identifier
    {
        get => identifier;
        set => identifier = value;
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
