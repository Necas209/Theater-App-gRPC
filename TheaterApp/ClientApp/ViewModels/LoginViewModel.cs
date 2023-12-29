using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public string UserName { get; set; } = string.Empty;

    public event StringMethod? ShowError;

    public async Task<bool> Login(SecureString password)
    {
        if (UserName is not { Length: > 0 } || password.Length == 0)
        {
            ShowError?.Invoke("Informação em falta.");
            return false;
        }

        var passwordHash = HashingService.HashPassword(password);
        var client = new AuthManager.AuthManagerClient(App.Channel);
        var reply = await client.LoginAsync(new LoginRequest
            {
                UserName = UserName,
                PasswordHash = passwordHash
            }
        );
        if (!reply.LoginStatus)
        {
            ShowError?.Invoke("Login failed!");
            return false;
        }

        App.UserId = reply.UserId;
        App.UserType = (User.UserType)reply.UserType;
        return true;
    }
}