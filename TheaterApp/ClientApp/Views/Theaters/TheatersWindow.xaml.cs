using System.Windows;
using ClientApp.ViewModels.Theaters;
using ClientApp.Views.Management;
using GrpcLibrary.Models;

namespace ClientApp.Views.Theaters;

public partial class TheatersWindow
{
    private readonly App _app;
    private readonly TheatersViewModel _model;

    public TheatersWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as TheatersViewModel)!;
        _model.ShowError += ShowError;
        if (_app.UserType == User.UserType.Manager)
            _model.IsManager = true;
        Dispatcher.Invoke(async () => await _model.GetTheaters(_app));
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetTheaters(_app));
    }

    private void BtAddTheater_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddTheaterWindow();
        window.ShowDialog();
    }

    private void BtEditTheater_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.Theater == null)
        {
            ShowError("Selecione um teatro primeiro.");
        }
        else
        {
            var window = new EditTheaterWindow(_model.Theater);
            window.ShowDialog();
        }
    }

    private void BtDelTheater_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.DelTheater(_app));
    }
}