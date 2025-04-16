using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogFilterCollection : Collection<CommonFileDialogFilter>
{
    internal CommonFileDialogFilterCollection()
    {
    }

    internal FilterSpec[] GetAllFilterSpecs()
    {
        var filterSpecs = new FilterSpec[Count];

        for (var i = 0; i < Count; i++)
        {
            filterSpecs[i] = this[i].GetFilterSpec();
        }

        return filterSpecs;
    }
}
