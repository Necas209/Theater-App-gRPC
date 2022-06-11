using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApp.ViewModels.Managers;

namespace ClientApp.Views.Management;

public partial class AddSessionWindow
{
    private readonly App _app;
    private readonly AddSessionViewModel _model;

    public AddSessionWindow()
    {
        InitializeComponent();
        _app = (App)Application.Current;
        _model = (AddSessionViewModel)DataContext;
        _model.ShowError += ShowError;
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void CbShows_OnKeyDown(object sender, KeyEventArgs e)
    {
        var text = ((ComboBox)sender).Text;
        if (e.Key == Key.Enter)
            Dispatcher.Invoke(async () => await _model.GetShows(_app, text));
    }

    private void CbTheaters_OnKeyDown(object sender, KeyEventArgs e)
    {
        var text = ((ComboBox)sender).Text;
        if (e.Key == Key.Enter)
            Dispatcher.Invoke(async () => await _model.GetTheaters(_app, text));
    }

    private void BtAddSession_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.AddSession(_app));
    }
}