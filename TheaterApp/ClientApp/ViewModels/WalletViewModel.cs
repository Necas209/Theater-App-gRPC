using System.ComponentModel.DataAnnotations;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class WalletViewModel : BaseViewModel
{
    private decimal _addedFunds;

    private Client _client = null!;

    private string? _paymentMethod;

    [DataType(DataType.Currency)]
    public decimal AddedFunds
    {
        get => _addedFunds;
        set
        {
            _addedFunds = value;
            OnPropertyChanged(nameof(AddedFunds));
        }
    }

    public Client Client
    {
        get => _client;
        set
        {
            _client = value;
            OnPropertyChanged(nameof(Client));
        }
    }

    public string? PaymentMethod
    {
        get => _paymentMethod;
        set
        {
            _paymentMethod = value;
            OnPropertyChanged(_paymentMethod);
        }
    }
}