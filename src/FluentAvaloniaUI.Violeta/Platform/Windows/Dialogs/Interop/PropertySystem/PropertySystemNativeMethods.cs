using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;

[SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public static class PropertySystemNativeMethods
{
    public enum RelativeDescriptionType
    {
        General,
        Date,
        Size,
        Count,
        Revision,
        Length,
        Duration,
        Speed,
        Rate,
        Rating,
        Priority
    }

    [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int PSGetNameFromPropertyKey(
        ref PropertyKey propkey,
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszCanonicalName
    );

    [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern HResult PSGetPropertyDescription(
        ref PropertyKey propkey,
        ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv
    );

    [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int PSGetPropertyDescriptionListFromString(
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropList,
        [In] ref Guid riid,
        out IPropertyDescriptionList ppv
    );

    [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int PSGetPropertyKeyFromName(
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName,
        out PropertyKey propkey
    );
}
