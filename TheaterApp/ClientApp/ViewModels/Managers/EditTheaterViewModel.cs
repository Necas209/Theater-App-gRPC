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
            OnPropertyChanged(nameof(Name));
        }
    }

    public string? Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged(nameof(Location));
        }
    }

    public string? Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged(nameof(Address));
        }
    }

    [DataType(DataType.EmailAddress)]
    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task SaveTheater()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta");
        }
        else if (string.IsNullOrWhiteSpace(Location))
        {
            ShowError?.Invoke("Localização em falta");
        }
        else if (string.IsNullOrWhiteSpace(Address))
        {
            ShowError?.Invoke("Endereço em falta");
        }
        else if (string.IsNullOrWhiteSpace(Email))
        {
            ShowError?.Invoke("Email em falta");
        }
        else if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowError?.Invoke("Telefone em falta");
        }
        else
        {
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
            if (!reply.Result) ShowError?.Invoke(reply.Description);
            else ShowMsg?.Invoke(reply.Description);
        }
    }
}