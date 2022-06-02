using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Windows;
using Client.ViewModels;
using Greet;
using GrpcLibrary.Models;

namespace Client.Views;

public partial class ReservationsWindow
{
    private readonly App _app;
    private readonly ReservationsViewModel _model;

    public ReservationsWindow()
    {
        _app = (Application.Current as App)!;
        _model = (DataContext as ReservationsViewModel)!;
        GetReservations();
        InitializeComponent();
    }

    private void GetReservations()
    {
        Dispatcher.Invoke(async () =>
        {
            var client = new ClientManager.ClientManagerClient(_app.Channel);
            var reply = await client.GetReservationsAsync(
                new GetReservationsRequest
                {
                    UserId = _app.UserId,
                    EndDate = _model.EndDate.ToString(CultureInfo.InvariantCulture),
                    StartDate = _model.StartDate.ToString(CultureInfo.InvariantCulture)
                }
            );
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(reply.Reservations);
            _model.Reservations.Clear();
            reservations?.ForEach(x => _model.Reservations.Add(x));
        });
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        if (_model.EndDate > DateTime.Now)
        {
            MessageBox.Show("Invalid end date!");
        }
        else
        {
            GetReservations();
        }
    }
}