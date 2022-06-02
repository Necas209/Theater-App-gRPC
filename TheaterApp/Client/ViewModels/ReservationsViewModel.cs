using System;
using System.Collections.ObjectModel;
using GrpcLibrary.Models;

namespace Client.ViewModels;

public class ReservationsViewModel
{
    public ReservationsViewModel()
    {
        Reservations = new ObservableCollection<Reservation>();
        EndDate = DateTime.Now;
        StartDate = EndDate.AddMonths(-3);
    }

    public ObservableCollection<Reservation> Reservations { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}