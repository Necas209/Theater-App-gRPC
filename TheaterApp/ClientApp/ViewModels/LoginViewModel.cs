using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public string UserName { get; set; } = null!;

    public event StringMethod? ShowError;

    public async Task<bool> Login(SecureString password)
    {
        if (string.IsNullOrWhiteSpace(UserName) || password.Length == 0)
        {
            ShowError?.Invoke("Informação em falta.");
        }
        else
        {
            var passwordHash = HashingService.HashPassword(password);
            var client = new AuthManager.AuthManagerClient(App.Channel);
            var reply = await client.LoginAsync(new LoginRequest
                {
                    UserName = UserName,
                    PasswordHash = passwordHash
                }
            );
            if (reply.LoginStatus)
            {
                App.UserId = reply.UserId;
                App.UserType = (User.UserType)reply.UserType;
            }
            else
            {
                ShowError?.Invoke("Login failed!");
            }

            return reply.LoginStatus;
        }

        return false;
    }
}