using System.Windows;
using ClientApp.ViewModels.Managers;
using GrpcLibrary.Models;

namespace ClientApp.Views.Management;

public partial class EditShowWindow
{
    private readonly App _app;
    private readonly EditShowViewModel _model;

    public EditShowWindow(Show show)
    {
        InitializeComponent();
        _app = (App)Application.Current;
        _model = new EditShowViewModel
        {
            Name = show.Name,
            Synopsis = show.Synopsis,
            Length = show.Length
        };
        DataContext = _model;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () => await _model.GetGenres(_app));
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtEditShow_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.SaveShow(_app));
    }
}