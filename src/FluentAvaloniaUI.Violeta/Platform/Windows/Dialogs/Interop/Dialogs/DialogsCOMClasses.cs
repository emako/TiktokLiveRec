using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;

[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeCommonFileDialog;

[ComImport]
[Guid(ShellIIDGuid.IFileOpenDialog)]
[CoClass(typeof(FileOpenDialogRCW))]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeFileOpenDialog : IFileOpenDialog;

[ComImport]
[Guid(ShellIIDGuid.IFileSaveDialog)]
[CoClass(typeof(FileSaveDialogRCW))]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeFileSaveDialog : IFileSaveDialog;

[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.FileOpenDialog)]
internal class FileOpenDialogRCW;

[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.FileSaveDialog)]
internal class FileSaveDialogRCW;
