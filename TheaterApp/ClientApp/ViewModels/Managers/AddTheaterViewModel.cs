using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Managers;

public partial class AddTheaterViewModel : BaseViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    [DataType(DataType.EmailAddress)] public string Email { get; set; } = string.Empty;

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; } = string.Empty;

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task AddTheater(App app)
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

        if (!MyRegex().IsMatch(PhoneNumber))
        {
            ShowError?.Invoke("Telefone em falta ou inválido.");
            return;
        }

        if (!MailAddress.TryCreate(Email, out _))
        {
            ShowError?.Invoke("Email em falta ou inválido.");
            return;
        }

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
        var eventCalled = reply.Result ? ShowMsg : ShowError;
        eventCalled?.Invoke(reply.Description);
    }

    [GeneratedRegex(@"^(?:[92]\d{2}(?:\s?\d{3}){2})$")]
    private static partial Regex MyRegex();
}