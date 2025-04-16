using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
[Serializable]
public class ShellException : ExternalException
{
    public ShellException()
    {
    }

    public ShellException(string message)
        : base(message)
    {
    }

    public ShellException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ShellException(string message, int errorCode)
        : base(message, errorCode)
    {
    }

    public ShellException(int errorCode)
        : base(LocalizedMessages.ShellExceptionDefaultText, errorCode)
    {
    }

    internal ShellException(HResult result)
        : this((int)result)
    {
    }

    internal ShellException(string message, HResult errorCode)
        : this(message, (int)errorCode)
    {
    }

    protected ShellException(SerializationInfo info, StreamingContext context)
#pragma warning disable SYSLIB0051 // Type or member is obsolete
        : base(info, context)
#pragma warning restore SYSLIB0051 // Type or member is obsolete
    {
    }
}
