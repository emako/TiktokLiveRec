using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;

public interface IKnownFolder : IDisposable, IEnumerable<ShellObject>
{
    public string CanonicalName { get; }

    public FolderCategory Category { get; }

    public DefinitionOptions DefinitionOptions { get; }

    public string Description { get; }

    public FileAttributes FileAttributes { get; }

    public Guid FolderId { get; }

    public string FolderType { get; }

    public Guid FolderTypeId { get; }

    public string LocalizedName { get; }

    public string LocalizedNameResourceId { get; }

    public Guid ParentId { get; }

    public string ParsingName { get; }

    public string Path { get; }

    public bool PathExists { get; }

    public RedirectionCapability Redirection { get; }

    public string RelativePath { get; }

    public string Security { get; }

    public string Tooltip { get; }

    public string TooltipResourceId { get; }
}
