﻿using System.Windows;
using ClientApp.ViewModels.Theaters;
using ClientApp.Views.Management;

namespace ClientApp.Views.Theaters;

public partial class ShowsWindow
{
    private readonly ShowsViewModel _model;

    public ShowsWindow()
    {
        InitializeComponent();
        _model = (DataContext as ShowsViewModel)!;
        _model.ShowError += ShowError;
        Dispatcher.Invoke(async () =>
        {
            await _model.GetShows();
            await _model.GetGenres();
        });
    }

    private static void ShowError(string s)
    {
        MessageBox.Show(s, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.GetShows());
    }

    private void BtAddShow_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AddShowWindow();
        window.ShowDialog();
    }

    private void BtEditShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.Show == null)
        {
            ShowError("Escolha um espetáculo primeiro");
        }
        else
        {
            var window = new EditShowWindow(_model.Show);
            window.ShowDialog();
        }
    }

    private void BtDelShow_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(async () => await _model.DelShow());
    }
}