using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientApp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public delegate void StringMethod(string s);

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}