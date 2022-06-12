using System.Windows;
using ClientApp.ViewModels.Admins;

namespace ClientApp.Views.Admins;

public partial class PurchasesWindow
{
    private readonly PurchasesViewModel _model;

    public PurchasesWindow()
    {
        InitializeComponent();
        _model = (PurchasesViewModel)DataContext;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetPurchases());
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetPurchases());
    }
}