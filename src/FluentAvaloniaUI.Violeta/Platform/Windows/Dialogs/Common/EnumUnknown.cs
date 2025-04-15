using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

internal class EnumUnknownClass : IEnumUnknown
{
    private readonly List<ICondition> conditionList = [];
    private int current = -1;

    internal EnumUnknownClass(ICondition[] conditions) => conditionList.AddRange(conditions);

    public HResult Clone(out IEnumUnknown result)
    {
        result = new EnumUnknownClass([.. conditionList]);
        return HResult.Ok;
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public HResult Next(uint requestedNumber, ref nint buffer, ref uint fetchedNumber)
    {
        current++;

        if (current < conditionList.Count)
        {
            buffer = Marshal.GetIUnknownForObject(conditionList[current]);
            fetchedNumber = 1;
            return HResult.Ok;
        }

        return HResult.False;
    }

    public HResult Reset()
    {
        current = -1;
        return HResult.Ok;
    }

    public HResult Skip(uint number)
    {
        var temp = current + (int)number;

        if (temp > (conditionList.Count - 1))
        {
            return HResult.False;
        }

        current = temp;
        return HResult.Ok;
    }
}
