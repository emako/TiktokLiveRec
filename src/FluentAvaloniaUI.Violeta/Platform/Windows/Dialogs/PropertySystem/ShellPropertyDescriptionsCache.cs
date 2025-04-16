using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

#pragma warning disable CS8618

[SupportedOSPlatform("Windows")]
internal class ShellPropertyDescriptionsCache
{
    private static ShellPropertyDescriptionsCache cacheInstance;

    private readonly IDictionary<PropertyKey, ShellPropertyDescription> propsDictionary = null!;

    private ShellPropertyDescriptionsCache()
    {
        propsDictionary = new Dictionary<PropertyKey, ShellPropertyDescription>();
    }

    public static ShellPropertyDescriptionsCache Cache
    {
        get
        {
            cacheInstance ??= new ShellPropertyDescriptionsCache();
            return cacheInstance;
        }
    }

    [SuppressMessage("Performance", "CA1854:Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method")]
    public ShellPropertyDescription GetPropertyDescription(PropertyKey key)
    {
        if (!propsDictionary.ContainsKey(key))
        {
            propsDictionary.Add(key, new ShellPropertyDescription(key));
        }
        return propsDictionary[key];
    }
}
