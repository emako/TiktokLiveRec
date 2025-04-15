using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellThumbnail
{
    private readonly IShellItem shellItemNative;

    private SIZE currentSize = new(256, 256);

    private ShellThumbnailFormatOption formatOption = ShellThumbnailFormatOption.Default;

    internal ShellThumbnail(ShellObject shellObject)
    {
        if (shellObject == null! || shellObject.NativeShellItem == null)
        {
            throw new ArgumentNullException(nameof(shellObject));
        }

        shellItemNative = shellObject.NativeShellItem;
    }

    public bool AllowBiggerSize { get; set; }

    public SIZE CurrentSize
    {
        get => currentSize;
        set
        {
            if (value.Height == 0 || value.Width == 0)
            {
                throw new System.ArgumentOutOfRangeException("value", LocalizedMessages.ShellThumbnailSizeCannotBe0);
            }

            var size = (FormatOption == ShellThumbnailFormatOption.IconOnly) ?
                DefaultIconSize.Maximum : DefaultThumbnailSize.Maximum;

            if (value.Height > size.Height || value.Width > size.Width)
            {
                throw new System.ArgumentOutOfRangeException("value",
                    string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    LocalizedMessages.ShellThumbnailCurrentSizeRange, size.ToString()));
            }

            currentSize = value;
        }
    }

    public ShellThumbnailFormatOption FormatOption
    {
        get => formatOption;
        set
        {
            formatOption = value;

            if (FormatOption == ShellThumbnailFormatOption.IconOnly
                && (CurrentSize.Height > DefaultIconSize.Maximum.Height || CurrentSize.Width > DefaultIconSize.Maximum.Width))
            {
                CurrentSize = DefaultIconSize.Maximum;
            }
        }
    }

    public ShellThumbnailRetrievalOption RetrievalOption { get; set; }
}
