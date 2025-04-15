using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.ExplorerBrowser;

[Serializable]
public class CommonControlException : COMException
{
    public CommonControlException()
    {
    }

    public CommonControlException(string message) : base(message)
    {
    }

    public CommonControlException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CommonControlException(string message, int errorCode) : base(message, errorCode)
    {
    }

    internal CommonControlException(string message, HResult errorCode) : this(message, (int)errorCode)
    {
    }

    protected CommonControlException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
#pragma warning disable SYSLIB0051 // Type or member is obsolete
        : base(info, context)
#pragma warning restore SYSLIB0051 // Type or member is obsolete
    {
    }
}
