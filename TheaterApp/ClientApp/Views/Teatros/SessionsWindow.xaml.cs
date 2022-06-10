using System.Windows;
using ClientApp.ViewModels;
using ClientApp.Views.Gestão;
using GrpcLibrary.Models;

namespace ClientApp.Views.Teatros;

public partial class SessionsWindow
{
    private readonly App _app;
    private readonly SessionsViewModel _model;

    public SessionsWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as SessionsViewModel)!;
        _model.ShowError += ShowError;
        if (_app.UserType == User.UserType.Manager)
            _model.IsManager = true;
        Dispatcher.Invoke(async () => await _model.GetSessions(_app));
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetSessions(_app));
    }

    private void BtAddSession_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddSessionsWindow();
        window.ShowDialog();
    }

    private void BtDelSession_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.DelSession(_app));
    }
}