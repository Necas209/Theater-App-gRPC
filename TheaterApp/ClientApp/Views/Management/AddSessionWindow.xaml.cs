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

    private async void CbShows_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not ComboBox { Text: { Length: > 0 } text })
            return;

        if (e.Key == Key.Enter)
            await _model.GetShows(text);
    }

    private async void CbTheaters_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not ComboBox { Text: { Length: > 0 } text })
            return;

        if (e.Key == Key.Enter)
            await _model.GetTheaters(text);
    }

    private async void BtAddSession_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.AddSession();
    }
}