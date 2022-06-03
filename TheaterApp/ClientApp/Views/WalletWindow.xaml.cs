using System.Text.Json;
using System.Windows;
using ClientApp.ViewModels;
using GrpcLibrary.Models;

namespace ClientApp.Views;

public partial class WalletWindow
{
    private readonly App _app;
    private readonly WalletViewModel _model;

    public WalletWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
        _model = (WalletViewModel)DataContext;
        Dispatcher.Invoke(async () =>
        {
            var clientManager = new ClientManager.ClientManagerClient(_app.Channel);
            var reply = await clientManager.GetClientInfoAsync(new GetClientInfoRequest
            {
                UserId = _app.UserId
            });
            _model.Client = JsonSerializer.Deserialize<Client>(reply.ClientInfo)!;
        });
    }

    private void BtAddFunds_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.AddedFunds <= 0)
            MessageBox.Show($"Fundos adicionados deverão ser superiores a {0:C}!");
        else if (_model.PaymentMethod == null)
            MessageBox.Show("Deverá selecionar um método de pagamento.");
        else
            Dispatcher.Invoke(async () =>
            {
                var client = new ClientManager.ClientManagerClient(_app.Channel);
                var reply = await client.AddFundsAsync(new AddFundsRequest
                {
                    UserId = _app.UserId,
                    Funds = (double)_model.AddedFunds,
                    PaymentMethod = _model.PaymentMethod
                });
                if (!reply.Result)
                    MessageBox.Show("Erro", reply.Description, 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    _model.Client.Funds = (decimal)reply.TotalFunds;
                    _model.Client = _model.Client; // Trigger change in 'Funds' label
                }
            });
    }
}