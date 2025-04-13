using Avalonia.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentAvalonia.UI.Controls;

public partial class ToastControl : UserControl
{
    public class ToastControlReactiveObject : INotifyPropertyChanged
    {
        private string _message = null!;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _imageGlyph = null!;

        public string ImageGlyph
        {
            get => _imageGlyph;
            set
            {
                if (SetProperty(ref _imageGlyph, value))
                    OnPropertyChanged(nameof(HasIcon));
            }
        }

        public bool HasIcon => ImageGlyph is not null;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public ToastControlReactiveObject ViewModel { get; }

    public string Message
    {
        get => ViewModel.Message;
        set => ViewModel.Message = value;
    }

    public string ImageGlyph
    {
        get => ViewModel.ImageGlyph;
        set => ViewModel.ImageGlyph = value;
    }

    public ToastControl()
    {
        DataContext = ViewModel = new();
        InitializeComponent();
    }
}
