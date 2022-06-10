using System.Windows;
using ClientApp.ViewModels;

namespace ClientApp.Views.Gestão;

public partial class AddShowsWindow
{
    private readonly App _app;
    private readonly AddShowViewModel _model;
    
    public AddShowsWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as AddShowViewModel)!;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetGenres(_app));
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.AddShow(_app));
    }
}