using System.Windows;
using ClientApp.ViewModels.Managers;
using GrpcLibrary.Models;

namespace ClientApp.Views.Management;

public partial class EditShowWindow
{
    private readonly EditShowViewModel _model;

    public EditShowWindow(Show show)
    {
        InitializeComponent();
        _model = (EditShowViewModel)DataContext;
        {
            _model.Name = show.Name;
            _model.Synopsis = show.Synopsis;
            _model.Length = show.Length;
            _model.Genre = show.Genre;
        }
        _model.ShowError += ShowError;
        _model.ShowMsg += ShowMsg;
        Dispatcher.Invoke(async () => await _model.GetGenres());
    }

    private static void ShowMsg(string s)
    {
        MessageBox.Show(s, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private async void BtEditShow_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.SaveShow();
    }
}