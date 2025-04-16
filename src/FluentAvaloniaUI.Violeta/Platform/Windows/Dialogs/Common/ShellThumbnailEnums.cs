using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public enum ShellThumbnailFormatOption
{
    Default,
    ThumbnailOnly = SIIGBF.ThumbnailOnly,
    IconOnly = SIIGBF.IconOnly,
}

[SupportedOSPlatform("Windows")]
public enum ShellThumbnailRetrievalOption
{
    Default,
    CacheOnly = SIIGBF.InCacheOnly,
    MemoryOnly = SIIGBF.MemoryOnly,
}
