using System.Windows;
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

    private void BtFilterTheaters_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetTheaters());
    }

    private void BtFilterShows_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void BtFilterSessions_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}