using System.ComponentModel.DataAnnotations;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class WalletViewModel : BaseViewModel
{
    private Client _client = null!;

    public Client Client
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
}