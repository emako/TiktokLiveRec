using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public struct IconReference
{
    private static readonly char[] commaSeparator = [','];
    private string moduleName;
    private string referencePath;

    public IconReference(string moduleName, int resourceId) : this()
    {
        if (string.IsNullOrEmpty(moduleName))
        {
            throw new ArgumentNullException(nameof(moduleName));
        }

        this.moduleName = moduleName;
        ResourceId = resourceId;
        referencePath = string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "{0},{1}", moduleName, resourceId);
    }

    public IconReference(string refPath) : this()
    {
        if (string.IsNullOrEmpty(refPath))
        {
            throw new ArgumentNullException(nameof(refPath));
        }

        var refParams = refPath.Split(commaSeparator);

        if (refParams.Length != 2 || string.IsNullOrEmpty(refParams[0]) || string.IsNullOrEmpty(refParams[1]))
        {
            throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(refPath));
        }

        moduleName = refParams[0];
        ResourceId = int.Parse(refParams[1], CultureInfo.InvariantCulture);

        referencePath = refPath;
    }

    public string ModuleName
    {
        readonly get => moduleName;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            moduleName = value;
        }
    }

    public string ReferencePath
    {
        readonly get => referencePath;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var refParams = value.Split(commaSeparator);

            if (refParams.Length != 2 || string.IsNullOrEmpty(refParams[0]) || string.IsNullOrEmpty(refParams[1]))
            {
                throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(value));
            }

            ModuleName = refParams[0];
            ResourceId = int.Parse(refParams[1], CultureInfo.InvariantCulture);

            referencePath = value;
        }
    }

    public int ResourceId { get; set; }

    public static bool operator !=(IconReference icon1, IconReference icon2) => !(icon1 == icon2);

    public static bool operator ==(IconReference icon1, IconReference icon2) => (icon1.moduleName == icon2.moduleName) &&
            (icon1.referencePath == icon2.referencePath) &&
            (icon1.ResourceId == icon2.ResourceId);

    [SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
    public override bool Equals(object obj)
    {
        if (obj == null || obj is not IconReference) { return false; }
        return (this == (IconReference)obj);
    }

    [SuppressMessage("Style", "IDE0070:Use 'System.HashCode'")]
    [SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
    public override int GetHashCode()
    {
        var hash = moduleName.GetHashCode();
        hash = hash * 31 + referencePath.GetHashCode();
        hash = hash * 31 + ResourceId.GetHashCode();
        return hash;
    }
}
