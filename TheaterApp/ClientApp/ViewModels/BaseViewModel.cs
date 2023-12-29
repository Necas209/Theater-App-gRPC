using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ClientApp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public delegate void StringMethod(string s);

    protected readonly App App = (App)Application.Current;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}