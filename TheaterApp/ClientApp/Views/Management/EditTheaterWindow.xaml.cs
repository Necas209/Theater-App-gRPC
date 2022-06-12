using System.Windows;
using ClientApp.ViewModels.Managers;
using GrpcLibrary.Models;

namespace ClientApp.Views.Management;

public partial class EditTheaterWindow
{
    private readonly EditTheaterViewModel _model;

    public EditTheaterWindow(Theater theater)
    {
        InitializeComponent();
        _model = new EditTheaterViewModel
        {
            Name = theater.Name,
            Location = theater.Location,
            Address = theater.Address,
            Email = theater.Email,
            PhoneNumber = theater.PhoneNumber
        };
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

    private void BtSaveTheater_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.SaveTheater());
    }
}