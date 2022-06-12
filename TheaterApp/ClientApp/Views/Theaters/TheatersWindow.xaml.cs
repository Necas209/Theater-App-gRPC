using System.Windows;
using ClientApp.ViewModels.Theaters;
using ClientApp.Views.Management;

namespace ClientApp.Views.Theaters;

public partial class TheatersWindow
{
    private readonly TheatersViewModel _model;

    public TheatersWindow()
    {
        InitializeComponent();
        _model = (DataContext as TheatersViewModel)!;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetTheaters());
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetTheaters());
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
        Dispatcher.Invoke(async () => await _model.DelTheater());
    }
}