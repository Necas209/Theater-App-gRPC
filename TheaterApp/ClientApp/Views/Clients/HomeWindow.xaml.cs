using System.Windows;
using System.Windows.Controls;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class HomeWindow
{
    private readonly HomeViewModel _model;

    public HomeWindow()
    {
        InitializeComponent();
        _model = (HomeViewModel)DataContext;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () =>
        {
            await _model.GetClientInfo();
            await _model.GetTheaters();
            await _model.GetGenres();
        });
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtProfile_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ProfileWindow();
        window.ShowDialog();
    }

    private void BtFilterTheaters_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetTheaters());
    }

    private void BtFilterShows_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetShows());
    }

    private void BtFilterSessions_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetSessions());
    }

    private void LvTheaters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Dispatcher.Invoke(async () =>
        {
            await _model.GetShowsByTheater();
            TiTheaters.IsSelected = false;
            TiShows.IsSelected = true;
        });
    }

    private void LvShows_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Dispatcher.Invoke(async () =>
        {
            await _model.GetSessions();
            TiShows.IsSelected = false;
            TiSessions.IsSelected = true;
        });
    }

    private void BtBuyTickets_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.Session == null)
            ShowError("Selecione uma sessão primeiro");
        else
        {
            var window = new BuyTicketsWindow(_model.Session.Id);
            window.ShowDialog();
        }
    }
}