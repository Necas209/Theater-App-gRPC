using System.Windows;
using ClientApp.ViewModels.Admins;

namespace ClientApp.Views.Admins;

public partial class LogsWindow
{
    private readonly LogsViewModel _model;

    public LogsWindow()
    {
        InitializeComponent();
        _model = (LogsViewModel)DataContext;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetLogs());
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetLogs());
    }
}