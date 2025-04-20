using Avalonia.Threading;
using FluentAvaloniaUI.Violeta.Mvvm;

namespace FluentAvalonia.UI.Controls;

public partial class MessageBoxStandardViewModel : AbstractMessageBoxViewModel
{
    public event Action<MessageBoxResult> CloseRequested = null!;

    public MessageBoxResult Result { get; set; }

    public readonly MessageBoxResult _enterDefaultButton;
    public readonly MessageBoxResult _escDefaultButton;

    public MessageBoxStandardViewModel(MessageBoxStandardParams @params) : base(@params, @params.Icon)
    {
        _enterDefaultButton = @params.EnterDefaultButton;
        _escDefaultButton = @params.EscDefaultButton;

        Text = new MessageBoxButtonText();
        SetButtons(@params.ButtonDefinitions);
        ButtonClickCommand = new RelayCommand<object?>(o => ButtonClick(o!.ToString()!));
        EnterClickCommand = new RelayCommand(EnterClick);
        EscClickCommand = new RelayCommand(EscClick);
    }

    public MessageBoxButtonText Text { get; private set; }

    public bool IsOkShowed { get; private set; }
    public bool IsYesShowed { get; private set; }
    public bool IsNoShowed { get; private set; }
    public bool IsAbortShowed { get; private set; }
    public bool IsCancelShowed { get; private set; }

    public override string InputLabel { get; internal set; } = null!;
    public override string InputValue { get; set; } = null!;
    public override bool IsInputMultiline { get; internal set; }
    public override bool IsInputVisible { get; internal set; }

    public RelayCommand<object?> ButtonClickCommand { get; } = null!;
    public RelayCommand EnterClickCommand { get; } = null!;
    public RelayCommand EscClickCommand { get; } = null!;

    private void SetButtons(MessageBoxButton paramsButtonDefinitions)
    {
        switch (paramsButtonDefinitions)
        {
            case MessageBoxButton.OK:
                IsOkShowed = true;
                break;

            case MessageBoxButton.YesNo:
                IsYesShowed = true;
                IsNoShowed = true;
                break;

            case MessageBoxButton.OKCancel:
                IsOkShowed = true;
                IsCancelShowed = true;
                break;

            case MessageBoxButton.YesNoCancel:
                IsYesShowed = true;
                IsNoShowed = true;
                IsCancelShowed = true;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(paramsButtonDefinitions), paramsButtonDefinitions,
                    null);
        }
    }

    private void EscClick()
    {
        switch (_escDefaultButton)
        {
            case MessageBoxResult.Ok:
                ButtonClick(MessageBoxResult.Ok);
                return;

            case MessageBoxResult.Yes:
                ButtonClick(MessageBoxResult.Yes);
                return;

            case MessageBoxResult.No:
                ButtonClick(MessageBoxResult.No);
                return;

            case MessageBoxResult.Abort:
                ButtonClick(MessageBoxResult.Abort);
                return;

            case MessageBoxResult.Cancel:
                ButtonClick(MessageBoxResult.Cancel);
                return;

            case MessageBoxResult.None:
                ButtonClick(MessageBoxResult.None);
                return;

            default:
                ButtonClick(MessageBoxResult.None);
                return;
        }
    }

    private void EnterClick()
    {
        switch (_enterDefaultButton)
        {
            case MessageBoxResult.Ok:
                ButtonClick(MessageBoxResult.Ok);
                return;

            case MessageBoxResult.Yes:
                ButtonClick(MessageBoxResult.Yes);
                return;

            case MessageBoxResult.No:
                ButtonClick(MessageBoxResult.No);
                return;

            case MessageBoxResult.Abort:
                ButtonClick(MessageBoxResult.Abort);
                return;

            case MessageBoxResult.Cancel:
                ButtonClick(MessageBoxResult.Cancel);
                return;

            case MessageBoxResult.None:
                ButtonClick(MessageBoxResult.None);
                return;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Close()
    {
        CloseRequested?.Invoke(Result);
    }

    public void SetButtonResult(MessageBoxResult result)
    {
        Result = result;
    }

    public async void ButtonClick(string parameter)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            SetButtonResult(Enum.Parse<MessageBoxResult>(parameter.Trim(), true));
            Close();
        });
    }

    public async void ButtonClick(MessageBoxResult buttonResult)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            SetButtonResult(buttonResult);
            Close();
        });
    }
}
