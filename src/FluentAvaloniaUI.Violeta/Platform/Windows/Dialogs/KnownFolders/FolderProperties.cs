using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;

[StructLayout(LayoutKind.Sequential)]
internal struct FolderProperties
{
    internal string name;
    internal FolderCategory category;
    internal string canonicalName;
    internal string description;
    internal Guid parentId;
    internal string parent;
    internal string relativePath;
    internal string parsingName;
    internal string tooltipResourceId;
    internal string tooltip;
    internal string localizedName;
    internal string localizedNameResourceId;
    internal string iconResourceId;
    internal object icon;  //internal BitmapSource icon;
    internal DefinitionOptions definitionOptions;
    internal FileAttributes fileAttributes;
    internal Guid folderTypeId;
    internal string folderType;
    internal Guid folderId;
    internal string path;
    internal bool pathExists;
    internal RedirectionCapability redirection;
    internal string security;
}
