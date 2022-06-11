using System.Windows;
using ClientApp.ViewModels.Managers;

namespace ClientApp.Views.Management;

public partial class AddShowWindow
{
    private readonly App _app;
    private readonly AddShowViewModel _model;

    public AddShowWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as AddShowViewModel)!;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetGenres(_app));
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.AddShow(_app));
    }
}