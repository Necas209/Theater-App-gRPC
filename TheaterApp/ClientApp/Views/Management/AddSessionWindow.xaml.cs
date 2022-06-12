using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientApp.ViewModels.Managers;

namespace ClientApp.Views.Management;

public partial class AddSessionWindow
{
    private readonly AddSessionViewModel _model;

    public AddSessionWindow()
    {
        InitializeComponent();
        _model = (AddSessionViewModel)DataContext;
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

    private void CbShows_OnKeyDown(object sender, KeyEventArgs e)
    {
        var text = ((ComboBox)sender).Text;
        if (e.Key == Key.Enter)
            Dispatcher.Invoke(async () => await _model.GetShows(text));
    }

    private void CbTheaters_OnKeyDown(object sender, KeyEventArgs e)
    {
        var text = ((ComboBox)sender).Text;
        if (e.Key == Key.Enter)
            Dispatcher.Invoke(async () => await _model.GetTheaters(text));
    }

    private void BtAddSession_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () =>
        {
            await _model.AddSession();
        });
    }
}