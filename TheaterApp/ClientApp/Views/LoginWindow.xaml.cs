using System.Windows;
using System.Windows.Input;
using ClientApp.ViewModels;
using ClientApp.Views.Clients;
using GrpcLibrary.Models;

namespace ClientApp.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow
{
    private readonly App _app;
    private readonly LoginViewModel _model;

    public LoginWindow()
    {
        InitializeComponent();
        _app = (App)Application.Current;
        _model = (LoginViewModel)DataContext;
        _model.ShowError += ShowError;
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtLogin_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () =>
        {
            var result = await _model.Login(PbPassword.SecurePassword);
            if (result)
            {
                Window window = _app.UserType switch
                {
                    User.UserType.Client => new HomeWindow(),
                    _ => new ProfileWindow()
                };
                Hide();
                window.ShowDialog();
                Show();
            }
        });
    }

    private void PbPassword_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            BtLogin_OnClick(sender, e);
    }

    private void BtRegister_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new RegisterWindow();
        window.ShowDialog();
    }
}