using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GrpcLibrary.Models;

namespace ClientApp.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow
{
    private readonly App _app;

    public LoginWindow()
    {
        InitializeComponent();
        _app = (Application.Current as App)!;
    }

    private void BtLogin_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbUserName.Text) || string.IsNullOrWhiteSpace(PbPassword.Password))
            ShowError("Missing information!");
        else
            Dispatcher.Invoke(async () => await Login());
    }

    private void PbPassword_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
            BtLogin_OnClick(BtLogin, e);
    }

    private async Task Login()
    {
        var passwordHash = User.HashPassword(PbPassword.Password);
        var client = new AuthManager.AuthManagerClient(_app.Channel);
        var reply = await client.LoginAsync(new LoginRequest
            {
                UserName = TbUserName.Text,
                PasswordHash = passwordHash
            }
        );
        if (reply.LoginStatus)
        {
            MessageBox.Show("Login successful!", "Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
            _app.UserId = reply.UserId;
            _app.UserType = (User.UserType)reply.UserType;
        }
        else
        {
            ShowError("Login failed!");
        }
    }

    private static void ShowError(string s)
    {
        MessageBox.Show("Erro", s, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}