using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;

namespace ClientApp.ViewModels.Clients;

public class RegisterViewModel : BaseViewModel
{
    public string UserName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [DataType(DataType.EmailAddress)] public string Email { get; set; } = string.Empty;

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task RegisterUser(SecureString password, SecureString confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            ShowError?.Invoke("UserName em falta.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta.");
            return;
        }

        if (!MailAddress.TryCreate(Email, out _))
        {
            ShowError?.Invoke("Email em falta ou inválido.");
            return;
        }

        if (password.Length == 0 || confirmPassword.Length == 0)
        {
            ShowError?.Invoke("Password em falta.");
            return;
        }

        if (!HashingService.SecureStringEqual(password, confirmPassword))
        {
            ShowError?.Invoke("Passwords não coincidem.");
            return;
        }

        var client = new AuthManager.AuthManagerClient(App.Channel);
        var reply = await client.RegisterAsync(new RegisterRequest
        {
            Name = Name,
            Email = Email,
            UserName = UserName,
            PasswordHash = HashingService.HashPassword(password)
        });
        var eventHandler = reply.Result ? ShowMsg : ShowError;
        eventHandler?.Invoke(reply.Description);
    }
}