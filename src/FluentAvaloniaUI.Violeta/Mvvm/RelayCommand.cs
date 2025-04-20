using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FluentAvaloniaUI.Violeta.Mvvm;

public class RelayCommand : ICommand
{
    private readonly Action execute = null!;

    private readonly Func<bool>? canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute)
    {
        ArgumentNullException.ThrowIfNull(execute, nameof(execute));
        this.execute = execute;
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute, nameof(execute));
        ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        execute();
    }
}
