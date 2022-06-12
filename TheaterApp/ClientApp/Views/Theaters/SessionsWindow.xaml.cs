using System.Windows;
using ClientApp.ViewModels.Theaters;
using ClientApp.Views.Management;

namespace ClientApp.Views.Theaters;

public partial class SessionsWindow
{
    private readonly SessionsViewModel _model;

    public SessionsWindow()
    {
        InitializeComponent();
        _model = (DataContext as SessionsViewModel)!;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () => await _model.GetSessions());
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetSessions());
    }

    private void BtAddSession_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddSessionWindow();
        window.ShowDialog();
    }

    private void BtDelSession_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.DelSession());
    }
}