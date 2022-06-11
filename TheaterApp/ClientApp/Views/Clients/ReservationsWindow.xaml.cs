using System;
using System.Windows;
using ClientApp.ViewModels;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class ReservationsWindow
{
    private readonly App _app;
    private readonly ReservationsViewModel _model;

    public ReservationsWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
        _model = (DataContext as ReservationsViewModel)!;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => { await _model.GetReservations(_app); });
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => { await _model.GetReservations(_app); });
    }
}