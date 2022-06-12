using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Threading.Tasks;
using GrpcLibrary;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private string? _email;
    private string? _name;
    private string? _passwordHash;
    private string? _userName;

    public ProfileViewModel()
    {
        IsAdmin = App.UserType == User.UserType.Admin;
        IsManagerOrAdmin = IsAdmin || App.UserType == User.UserType.Manager;
        IsClient = !IsManagerOrAdmin;
    }

    public bool IsManagerOrAdmin { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsClient { get; set; }

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string? UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged(nameof(UserName));
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

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetUserInfo()
    {
        var client = new AuthManager.AuthManagerClient(App.Channel);
        var reply = await client.GetUserInfoAsync(new GetUserInfoRequest
        {
            UserId = App.UserId
        });
        Name = reply.Name;
        UserName = reply.UserName;
        Email = reply.Email;
        _passwordHash = reply.PasswordHash;
    }

    public async Task UpdateInfo(SecureString oldPassword, SecureString newPassword)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta.");
        }
        else if (string.IsNullOrWhiteSpace(Email))
        {
            ShowError?.Invoke("Email em falta.");
        }
        else if (HashingService.HashPassword(oldPassword) != _passwordHash)
        {
            ShowError?.Invoke("Password atual incorreta.");
        }
        else
        {
            var client = new AuthManager.AuthManagerClient(App.Channel);
            var reply = await client.UpdUserInfoAsync(new UpdUserInfoRequest
            {
                UserId = App.UserId,
                Name = Name,
                UserName = UserName,
                Email = Email,
                PasswordHash = HashingService.HashPassword(newPassword.Length > 0 ? newPassword : oldPassword)
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
            else
                ShowMsg?.Invoke(reply.Description);
        }
    }
}