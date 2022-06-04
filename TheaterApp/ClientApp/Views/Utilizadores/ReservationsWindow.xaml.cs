using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.Views;

public partial class ReservationsWindow
{
    private readonly App _app;

    public ObservableCollection<Reservation> Reservations { get; }

    public ReservationsWindow()
    {
        _app = (Application.Current as App)!;
        Reservations = new ObservableCollection<Reservation>();
        InitializeComponent();
        DpStartDate.SelectedDate = DateTime.Today.AddMonths(-3);
        DpEndDate.SelectedDate = DateTime.Today;
        GetReservations();
    }

    private void GetReservations()
    {
        if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            MessageBox.Show("Data de início/fim por selecionar!");
        else
            Dispatcher.Invoke(async () =>
            {
                var client = new ClientManager.ClientManagerClient(_app.Channel);
                var reply = await client.GetReservationsAsync(new GetReservationsRequest
                    {
                        UserId = _app.UserId,
                        StartDate = Timestamp.FromDateTime((DateTime)DpStartDate.SelectedDate),
                        EndDate = Timestamp.FromDateTime((DateTime)DpEndDate.SelectedDate)
                    }
                );
                var reservations = JsonSerializer.Deserialize<List<Reservation>>(reply.Reservations);
                Reservations.Clear();
                reservations?.ForEach(reservation => Reservations.Add(reservation));
            });
    }

    private void BtFilter_OnClick(object sender, RoutedEventArgs e)
    {
        if (DpEndDate.SelectedDate > DateTime.Now)
        {
            MessageBox.Show("Data de fim deve ser atual!");
        }
        else
        {
            GetReservations();
        }
    }
}