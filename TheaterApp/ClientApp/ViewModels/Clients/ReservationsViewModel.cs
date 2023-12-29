using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class ReservationsViewModel : BaseViewModel
{
    public ObservableCollection<Reservation> Reservations { get; } = [];

    public Reservation? Reservation { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-3);

    public DateTime EndDate { get; set; } = DateTime.Now;

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetReservations()
    {
        if (EndDate > DateTime.Today)
        {
            ShowError?.Invoke("Data de fim deve ser atual!");
            return;
        }

        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
            return;
        }

        var client = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await client.GetReservationsAsync(new GetReservationsRequest
            {
                UserId = App.UserId,
                StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(EndDate.AddDays(1).ToUniversalTime())
            }
        );
        var reservations = JsonSerializer.Deserialize<List<Reservation>>(reply.Reservations);
        Reservations.Clear();
        foreach (var reservation in reservations ?? Enumerable.Empty<Reservation>()) Reservations.Add(reservation);
    }

    public async Task CancelReservation()
    {
        if (Reservation == null)
        {
            ShowError?.Invoke("Selecione uma compra primeiro.");
            return;
        }

        var client = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await client.RefundAsync(new RefundRequest
        {
            UserId = App.UserId,
            ReservationId = Reservation.Id
        });
        if (!reply.Result)
        {
            ShowError?.Invoke(reply.Description);
            return;
        }

        ShowMsg?.Invoke(reply.Description);
        Reservations.Remove(Reservation);
        Reservation = null;
    }
}