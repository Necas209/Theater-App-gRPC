using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Managers;

public class EditTheaterViewModel : BaseViewModel
{
    private string? _address;
    private string? _email;
    private string? _location;
    private string? _name;
    private string? _phoneNumber;

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string? Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged();
        }
    }

    public string? Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged();
        }
    }

    [DataType(DataType.EmailAddress)]
    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged();
        }
    }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task SaveTheater()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta");
            return;
        }

        if (string.IsNullOrWhiteSpace(Location))
        {
            ShowError?.Invoke("Localização em falta");
            return;
        }

        if (string.IsNullOrWhiteSpace(Address))
        {
            ShowError?.Invoke("Endereço em falta");
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ShowError?.Invoke("Email em falta");
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowError?.Invoke("Telefone em falta");
            return;
        }

        var client = new MgrManager.MgrManagerClient(App.Channel);
        var reply = await client.EditTheaterAsync(new EditTheaterRequest
        {
            UserId = App.UserId,
            Name = Name,
            Location = Location,
            Address = Address,
            Email = Email,
            PhoneNumber = PhoneNumber
        });
        var eventHandler = reply.Result ? ShowMsg : ShowError;
        eventHandler?.Invoke(reply.Description);
    }
}