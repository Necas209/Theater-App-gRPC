using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class ReservationsViewModel : BaseViewModel
{
    public ReservationsViewModel()
    {
        StartDate = DateTime.Today.AddMonths(-3);
        EndDate = DateTime.Now;
        Reservations = new ObservableCollection<Reservation>();
    }

    public ObservableCollection<Reservation> Reservations { get; }

    public Reservation? Reservation { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetReservations()
    {
        if (EndDate > DateTime.Today)
        {
            ShowError?.Invoke("Data de fim deve ser atual!");
        }
        else if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
        }
        else
        {
            var client = new ClientManager.ClientManagerClient(App.Channel);
            var reply = await client.GetReservationsAsync(new GetReservationsRequest
                {
                    UserId = App.UserId,
                    StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
                    EndDate = Timestamp.FromDateTime(EndDate.ToUniversalTime())
                }
            );
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(reply.Reservations);
            Reservations.Clear();
            reservations?.ForEach(reservation => Reservations.Add(reservation));
        }
    }

    public async Task CancelReservation()
    {
        if (Reservation == null)
        {
            ShowError?.Invoke("Selecione uma compra primeiro.");
        }
        else
        {
            var client = new ClientManager.ClientManagerClient(App.Channel);
            var reply = await client.RefundAsync(new RefundRequest
            {
                UserId = App.UserId,
                ReservationId = Reservation.Id
            });
            if (!reply.Result)
            {
                ShowError?.Invoke(reply.Description);
            }
            else
            {
                ShowMsg?.Invoke(reply.Description);
                Reservations.Remove(Reservation);
                Reservation = null;
            }
        }
    }
}