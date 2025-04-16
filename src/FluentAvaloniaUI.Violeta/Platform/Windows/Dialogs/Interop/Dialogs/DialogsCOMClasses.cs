using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeCommonFileDialog;

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ShellIIDGuid.IFileOpenDialog)]
[CoClass(typeof(FileOpenDialogRCW))]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeFileOpenDialog : IFileOpenDialog;

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ShellIIDGuid.IFileSaveDialog)]
[CoClass(typeof(FileSaveDialogRCW))]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
internal interface NativeFileSaveDialog : IFileSaveDialog;

[SupportedOSPlatform("Windows")]
[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.FileOpenDialog)]
internal class FileOpenDialogRCW;

[SupportedOSPlatform("Windows")]
[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.FileSaveDialog)]
internal class FileSaveDialogRCW;
