namespace MicaSetup.Shell.Dialogs;

public class ShellSavedSearchCollection : ShellSearchCollection
{
    internal ShellSavedSearchCollection(IShellItem2 shellItem)
        : base(shellItem) => CoreHelpers.ThrowIfNotVista();
}
