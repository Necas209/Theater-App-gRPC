﻿using System.Windows;
using ClientApp.ViewModels.Managers;
using GrpcLibrary.Models;

namespace ClientApp.Views.Management;

public partial class EditTheaterWindow
{
    private readonly EditTheaterViewModel _model;

    public EditTheaterWindow(Theater theater)
    {
        InitializeComponent();
        _model = (EditTheaterViewModel)DataContext;
        _model.Name = theater.Name;
        _model.Location = theater.Location;
        _model.Address = theater.Address;
        _model.Email = theater.Email;
        _model.PhoneNumber = theater.PhoneNumber;
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

    private async void BtSaveTheater_OnClick(object sender, RoutedEventArgs e)
    {
        await _model.SaveTheater();
    }
}