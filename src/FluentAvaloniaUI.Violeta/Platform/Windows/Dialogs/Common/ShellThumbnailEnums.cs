using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public enum ShellThumbnailFormatOption
{
    Default,
    ThumbnailOnly = SIIGBF.ThumbnailOnly,
    IconOnly = SIIGBF.IconOnly,
}

public enum ShellThumbnailRetrievalOption
{
    Default,
    CacheOnly = SIIGBF.InCacheOnly,
    MemoryOnly = SIIGBF.MemoryOnly,
}
