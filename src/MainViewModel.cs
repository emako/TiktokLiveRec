using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputedConverters;
using System.Windows.Controls;

namespace TiktokLiveRec;

[ObservableObject]
public partial class MainViewModel : ReactiveObject
{
    public DataGrid RecTable { get; private set; } = null!;

    [RelayCommand]
    public void RecTableLoaded(RelayEventParameter param)
    {
        RecTable = (DataGrid)param.Deconstruct().Sender;
    }
}
