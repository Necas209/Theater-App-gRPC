using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Managers;

public class EditTheaterViewModel : BaseViewModel
{
    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Address { get; set; } = null!;

    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; } = null!;

    public event StringMethod? ShowError;

    public async Task SaveTheater(App app)
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
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.EditTheaterAsync(new EditTheaterRequest
            {
                UserId = app.UserId,
                Name = Name,
                Location = Location,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber
            });
            if (!reply.Result) ShowError?.Invoke(reply.Description);
        }
    }
}