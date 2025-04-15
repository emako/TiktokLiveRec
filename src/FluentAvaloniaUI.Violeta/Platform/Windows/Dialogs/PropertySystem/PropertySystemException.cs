using System.Runtime.InteropServices;

#pragma warning disable SYSLIB0051 // Type or member is obsolete

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

[Serializable]
public class PropertySystemException : ExternalException
{
    public PropertySystemException()
    {
    }

    public PropertySystemException(string message) : base(message)
    {
    }

    public PropertySystemException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public PropertySystemException(string message, int errorCode) : base(message, errorCode)
    {
    }

    protected PropertySystemException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
}
