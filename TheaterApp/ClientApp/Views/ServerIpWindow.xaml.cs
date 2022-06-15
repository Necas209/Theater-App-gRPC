using System.ComponentModel;
using System.Windows;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class ServerIpWindow
{
    private readonly App _app;
    private readonly ServerIpViewModel _model;

    public ServerIpWindow()
    {
        InitializeComponent();
        _app = (App)Application.Current;
        _model = (ServerIpViewModel)DataContext;
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtConnect_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_model.Connect()) return;
        var window = new LoginWindow();
        Hide();
        window.ShowDialog();
        Show();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        Dispatcher.Invoke(async () =>
        {
            if (_app.Channel != null)
                await _app.Channel.ShutdownAsync();
        });
        base.OnClosing(e);
    }
}