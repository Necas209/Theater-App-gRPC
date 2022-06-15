using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Managers;

public class AddTheaterViewModel : BaseViewModel
{
    public AddTheaterViewModel()
    {
        Name = "";
        Location = "";
        Address = "";
        Email = "";
        PhoneNumber = "";
    }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Address { get; set; }

    [DataType(DataType.EmailAddress)] public string Email { get; set; }

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task AddTheater(App app)
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
        else if (!Regex.IsMatch(PhoneNumber, "^(?:[92]\\d{2}(?:\\s?\\d{3}){2})$"))
        {
            ShowError?.Invoke("Telefone em falta ou inválido.");
        }
        else if (!MailAddress.TryCreate(Email, out _))
        {
            ShowError?.Invoke("Email em falta ou inválido.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.AddTheaterAsync(new AddTheaterRequest
            {
                UserId = app.UserId,
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