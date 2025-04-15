using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

internal static class IntPtrExtensions
{
    [SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known")]
    public static T MarshalAs<T>(this nint ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));
}
