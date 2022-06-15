using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Admins;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel()
    {
        Name = "";
        UserName = "";
        Email = "";
        UserType = User.UserType.Admin;
        UserTypes = new Dictionary<User.UserType, string>
        {
            { User.UserType.Admin, "Administrador" },
            { User.UserType.Manager, "Gestor" }
        };
    }

    public string UserName { get; set; }

    public string Name { get; set; }

    [DataType(DataType.EmailAddress)] public string Email { get; set; }

    public Dictionary<User.UserType, string> UserTypes { get; }

    public User.UserType UserType { get; set; }

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
            var client = new AdminManager.AdminManagerClient(App.Channel);
            var reply = await client.AddUserAsync(new AddUserRequest
            {
                UserId = App.UserId,
                Name = Name,
                Email = Email,
                UserName = UserName,
                PasswordHash = HashingService.HashPassword(password),
                UserType = (int)UserType
            });
            if (reply.Result)
                ShowMsg?.Invoke(reply.Description);
            else
                ShowError?.Invoke(reply.Description);
        }
    }
}