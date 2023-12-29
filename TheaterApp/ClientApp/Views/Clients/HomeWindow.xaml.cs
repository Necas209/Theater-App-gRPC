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
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () =>
        {
            await _model.GetClientInfo();
            await _model.GetTheaters();
            await _model.GetGenres();
        });
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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

    private async void BtFilterTheaters_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.GetTheaters();
    }

    private async void BtFilterShows_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.GetShows();
    }

    private async void BtFilterSessions_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.GetSessions();
    }

    private async void LvTheaters_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_model.Theater == null) return;
        await _model.GetShowsByTheater();
        TabTheaters.IsSelected = false;
        TabShows.IsSelected = true;
    }

    private async void LvShows_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_model.Show == null) return;
        await _model.GetSessions();
        TabShows.IsSelected = false;
        TabSessions.IsSelected = true;
    }

    private async void BtBuyTickets_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.Session == null)
        {
            ShowError("Selecione uma sessão primeiro");
            return;
        }

        var window = new BuyTicketsWindow(_model.Session.Id);
        window.ShowDialog();
        await _model.GetClientInfo();
    }

    private async void BtWatched_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.MarkAsWatched();
    }
}