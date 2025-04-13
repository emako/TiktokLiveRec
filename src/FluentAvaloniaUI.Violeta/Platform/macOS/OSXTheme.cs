using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

namespace FluentAvaloniaUI.Violeta.Platform.macOS;

[SupportedOSPlatform("macOS")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public static class OSXTheme
{
    [DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
    private static extern nint NSClassFromString(string name);

    [DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
    private static extern nint objc_msgSend(nint receiver, nint selector);

    [DllImport("/System/Library/Frameworks/Foundation.framework/Foundation", EntryPoint = "objc_msgSend")]
    private static extern nint objc_msgSend_nint(nint receiver, nint selector, nint arg1);

    private static readonly nint selStandardUserDefaults = Selector("standardUserDefaults");
    private static readonly nint selStringForKey = Selector("stringForKey:");
    private static readonly nint selUTF8String = Selector("UTF8String");

    private static nint Selector(string name) => sel_registerName(name);

    [DllImport("/usr/lib/libobjc.dylib")]
    private static extern nint sel_registerName(string selectorName);

    public static bool SystemUsesDarkTheme()
    {
        try
        {
            var nsUserDefaults = objc_msgSend(NSClassFromString("NSUserDefaults"), selStandardUserDefaults);
            var key = NSString("AppleInterfaceStyle");
            var valuePtr = objc_msgSend_nint(nsUserDefaults, selStringForKey, key);
            var utf8Ptr = objc_msgSend(valuePtr, selUTF8String);

            if (utf8Ptr != nint.Zero)
            {
                string value = Marshal.PtrToStringUTF8(utf8Ptr)!;
                return string.Equals(value, "Dark", StringComparison.OrdinalIgnoreCase);
            }
        }
        catch
        {
            // Ignore and default to Light
        }

        return false;
    }

    private static nint NSString(string str)
    {
        var cls = NSClassFromString("NSString");
        var selector = sel_registerName("stringWithUTF8String:");
        return objc_msgSend_nint(cls, selector, Marshal.StringToHGlobalAuto(str));
    }
}
