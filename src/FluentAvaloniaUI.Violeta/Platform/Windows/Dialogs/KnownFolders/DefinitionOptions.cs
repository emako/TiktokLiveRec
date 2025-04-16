using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;

[SupportedOSPlatform("Windows")]
[Flags]
public enum DefinitionOptions
{
    None = 0x0,
    LocalRedirectOnly = 0x2,
    Roamable = 0x4,
    Precreate = 0x8,
}
