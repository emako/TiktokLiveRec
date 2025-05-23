using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;

[SupportedOSPlatform("Windows")]
public enum FolderCategory
{
    None = 0x00,
    Virtual = 0x1,
    Fixed = 0x2,
    Common = 0x3,
    PerUser = 0x4,
}
