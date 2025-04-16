using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ShellIIDGuid.IShellLibrary)]
[CoClass(typeof(ShellLibraryCoClass))]
internal interface INativeShellLibrary : IShellLibrary;

[SupportedOSPlatform("Windows")]
[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.ShellLibrary)]
internal class ShellLibraryCoClass;
