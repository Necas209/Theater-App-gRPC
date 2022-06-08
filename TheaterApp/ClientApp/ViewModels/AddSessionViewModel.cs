using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class AddSessionViewModel : BaseViewModel
{
    public AddSessionViewModel()
    {
        Showtime = DateTime.Now;
        Shows = new ObservableCollection<Show>();
        Theaters = new ObservableCollection<Theater>();
    }

    public ObservableCollection<Show> Shows { get; set; }

    public ObservableCollection<Theater> Theaters { get; set; }

    public int ShowId { get; set; }

    public int TheaterId { get; set; }

    public DateTime Showtime { get; set; }

    [DataType(DataType.Currency)] public decimal TicketPrice { get; set; }

    public int TotalSeats { get; set; }
}