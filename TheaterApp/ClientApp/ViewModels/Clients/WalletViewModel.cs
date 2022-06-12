using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class WalletViewModel : BaseViewModel
{
    private Client? _client;

    public Client? Client
    {
        get => _client;
        set
        {
            _client = value;
            OnPropertyChanged(nameof(Client));
        }
    }

    [DataType(DataType.Currency)] public decimal AddedFunds { get; set; }

    public string? PaymentMethod { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetClientInfo()
    {
        var clientManager = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await clientManager.GetClientInfoAsync(new GetClientInfoRequest
        {
            UserId = App.UserId
        });
        Client = JsonSerializer.Deserialize<Client>(reply.ClientInfo);
    }

    public async Task AddFunds()
    {
        if (AddedFunds <= 0)
        {
            ShowError?.Invoke($"Fundos adicionados deverão ser superiores a {0:C}!");
        }
        else if (PaymentMethod == null)
        {
            ShowError?.Invoke("Deverá selecionar um método de pagamento.");
        }
        else
        {
            var client = new ClientManager.ClientManagerClient(App.Channel);
            var reply = await client.AddFundsAsync(new AddFundsRequest
            {
                UserId = App.UserId,
                Funds = AddedFunds,
                PaymentMethod = PaymentMethod
            });
            if (!reply.Result)
            {
                ShowError?.Invoke(reply.Description);
            }
            else
            {
                Client!.Funds = reply.TotalFunds;
                OnPropertyChanged(nameof(Client));
                ShowMsg?.Invoke(reply.Description);
            }
        }
    }
}