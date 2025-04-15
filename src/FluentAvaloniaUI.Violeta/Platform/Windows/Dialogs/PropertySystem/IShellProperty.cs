using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

public interface IShellProperty
{
    public string CanonicalName { get; }

    public ShellPropertyDescription Description { get; }

    public IconReference IconReference { get; }

    public PropertyKey PropertyKey { get; }

    public object ValueAsObject { get; }

    public Type ValueType { get; }

    public string FormatForDisplay(PropertyDescriptionFormatOptions format);
}
