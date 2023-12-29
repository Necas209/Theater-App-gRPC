using System.Windows;
using ClientApp.ViewModels;
using ClientApp.Views.Admins;
using ClientApp.Views.Clients;
using ClientApp.Views.Theaters;
using RegisterWindow = ClientApp.Views.Admins.RegisterWindow;

namespace ClientApp.Views;

public partial class ProfileWindow
{
    private readonly ProfileViewModel _model;

    public ProfileWindow()
    {
        InitializeComponent();
        _model = (ProfileViewModel)DataContext;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () => await _model.GetUserInfo());
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private async void BtSaveInfo_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.UpdateInfo(PbOldPassword.SecurePassword, PbNewPassword.SecurePassword);
    }


    private void BtReservations_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ReservationsWindow();
        window.ShowDialog();
    }

    private void BtWallet_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new WalletWindow();
        window.ShowDialog();
    }

    private void BtSessions_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new SessionsWindow();
        window.ShowDialog();
    }

    private void BtShows_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ShowsWindow();
        window.ShowDialog();
    }

    private void BtTheaters_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new TheatersWindow();
        window.ShowDialog();
    }

    private void BtPurchases_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new PurchasesWindow();
        window.ShowDialog();
    }

    private void BtLogs_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new LogsWindow();
        window.ShowDialog();
    }

    private void BtRegister_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new RegisterWindow();
        window.ShowDialog();
    }
}