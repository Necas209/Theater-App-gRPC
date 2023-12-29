using System.Windows;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class RegisterWindow
{
    private readonly RegisterViewModel _model;

    public RegisterWindow()
    {
        InitializeComponent();
        _model = (RegisterViewModel)DataContext;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void BtRegister_OnClick(object sender, RoutedEventArgs e)
    { 
        await _model.RegisterUser(PbPassword.SecurePassword, PbConfirmPassword.SecurePassword);
    }
}