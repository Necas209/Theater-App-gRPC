using System.Windows;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class BuyTicketsWindow
{
    private readonly BuyTicketsViewModel _model;

    public BuyTicketsWindow(int sessionId)
    {
        InitializeComponent();
        _model = (BuyTicketsViewModel)DataContext;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () =>
        {
            await _model.GetClientInfo();
            await _model.GetSession(sessionId);
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

    private void BtBuyTickets_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.ButTickets());
    }
}