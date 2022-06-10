using System.Windows;
using ClientApp.ViewModels;
using ClientApp.Views.Gestão;
using GrpcLibrary.Models;

namespace ClientApp.Views.Teatros;

public partial class ShowsWindow
{
    private readonly App _app;
    private readonly ShowsViewModel _model;

    public ShowsWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as ShowsViewModel)!;
        _model.ShowError += ShowError;
        if (_app.UserType == User.UserType.Manager)
            _model.IsManager = true;
        Dispatcher.Invoke(async () =>
        {
            await _model.GetShows(_app);
            await _model.GetGenres(_app);
        });
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetShows(_app));
    }

    private void BtAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddShowsWindow();
        window.ShowDialog();
    }

    private void BtEditShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.Show == null)
        {
            ShowError("Escolha um espetáculo primeiro");
        }
        else
        {
            var window = new EditShowsWindow(_model.Show);
            window.ShowDialog();
        }
    }

    private void BtDelShow_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.DelShow(_app));
    }
}