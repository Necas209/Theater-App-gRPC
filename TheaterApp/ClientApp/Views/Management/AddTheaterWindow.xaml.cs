using System.Windows;
using ClientApp.ViewModels.Managers;

namespace ClientApp.Views.Management;

public partial class AddTheaterWindow
{
    private readonly App _app;
    private readonly AddTheaterViewModel _model;

    public AddTheaterWindow()
    {
        InitializeComponent();
        _app = (App)Application.Current;
        _model = (AddTheaterViewModel)DataContext;
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

    private void BtAddTheater_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.AddTheater(_app));
    }
}