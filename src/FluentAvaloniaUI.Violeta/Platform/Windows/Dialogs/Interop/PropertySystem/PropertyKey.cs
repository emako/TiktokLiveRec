using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
[SuppressMessage("Style", "IDE0044:Add readonly modifier")]
[SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
public struct PropertyKey : IEquatable<PropertyKey>
{
    private Guid formatId;
    private readonly int propertyId;

    public Guid FormatId => formatId;

    public int PropertyId => propertyId;

    public PropertyKey(Guid formatId, int propertyId)
    {
        this.formatId = formatId;
        this.propertyId = propertyId;
    }

    public PropertyKey(string formatId, int propertyId)
    {
        this.formatId = new Guid(formatId);
        this.propertyId = propertyId;
    }

    public bool Equals(PropertyKey other) => other.Equals((object)this);

    public override int GetHashCode() => formatId.GetHashCode() ^ propertyId;

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is not PropertyKey)
            return false;

        var other = (PropertyKey)obj;
        return other.formatId.Equals(formatId) && (other.propertyId == propertyId);
    }

    public static bool operator ==(PropertyKey propKey1, PropertyKey propKey2) => propKey1.Equals(propKey2);

    public static bool operator !=(PropertyKey propKey1, PropertyKey propKey2) => !propKey1.Equals(propKey2);

    public override string ToString() => string.Format(CultureInfo.InvariantCulture,
            LocalizedMessages.PropertyKeyFormatString,
            formatId.ToString("B"), propertyId);
}
