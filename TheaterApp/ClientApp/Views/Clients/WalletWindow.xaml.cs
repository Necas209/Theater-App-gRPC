using System.Windows;
using ClientApp.ViewModels.Clients;

namespace ClientApp.Views.Clients;

public partial class WalletWindow
{
    private readonly WalletViewModel _model;

    public WalletWindow()
    {
        InitializeComponent();
        _model = (WalletViewModel)DataContext;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () => await _model.GetClientInfo());
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private async void BtAddFunds_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.AddFunds();
    }
}