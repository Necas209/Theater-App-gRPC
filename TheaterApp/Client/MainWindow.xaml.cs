using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Greet;
using GrpcLibrary.Models;

namespace Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly App _app;

    public MainWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
    }

    private void BtLogin_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbUserName.Text) || string.IsNullOrWhiteSpace(PbPassword.Password))
        {
            MessageBox.Show("Missing information!", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            Dispatcher.Invoke(async () => await Login());
        }
    }
    
    private void PbPassword_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
        {
            BtLogin_OnClick(BtLogin, e);
        }
    }
    
    private async Task Login()
    {
        var passwordHash = User.HashPassword(PbPassword.Password);
        var client = new AuthManager.AuthManagerClient(_app.Channel);
        var reply = await client.LoginAsync(
            new LoginRequest
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
        }
        else
        {
            MessageBox.Show("Login failed!", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}