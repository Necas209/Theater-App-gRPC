using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;

namespace ClientApp.ViewModels.Clients;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel()
    {
        UserName = "";
        Name = "";
        Email = "";
    }

    public string UserName { get; set; }

    public string Name { get; set; }

    [DataType(DataType.EmailAddress)] public string Email { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task RegisterUser(SecureString password, SecureString confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            ShowError?.Invoke("UserName em falta.");
        }
        else if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta.");
        }
        else if (!MailAddress.TryCreate(Email, out _))
        {
            ShowError?.Invoke("Email em falta ou inválido.");
        }
        else if (password.Length == 0 || confirmPassword.Length == 0)
        {
            ShowError?.Invoke("Password em falta.");
        }
        else if (!HashingService.SecureStringEqual(password, confirmPassword))
        {
            ShowError?.Invoke("Passwords não coincidem.");
        }
        else
        {
            var client = new AuthManager.AuthManagerClient(App.Channel);
            var reply = await client.RegisterAsync(new RegisterRequest
            {
                Name = Name,
                Email = Email,
                UserName = UserName,
                PasswordHash = HashingService.HashPassword(password)
            });
            if (reply.Result)
                ShowMsg?.Invoke(reply.Description);
            else
                ShowError?.Invoke(reply.Description);
        }
    }
}