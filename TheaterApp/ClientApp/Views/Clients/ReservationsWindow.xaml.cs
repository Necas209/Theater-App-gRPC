using System.Windows;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class ReservationsWindow
{
    private readonly ReservationsViewModel _model;

    public ReservationsWindow()
    {
        InitializeComponent();
        _model = (DataContext as ReservationsViewModel)!;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () => await _model.GetReservations());
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void ShowError(string s)
    {
        MessageBox.Show( s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private async void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.GetReservations();
    }

    private async void BtCancelReservation_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.CancelReservation();
    }
}