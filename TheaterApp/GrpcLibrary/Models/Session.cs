using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Session
{
    public Session()
    {
        Reservations = new HashSet<Reservation>();
    }

    [Key] public int Id { get; set; }

    public int ShowId { get; set; }

    public int TheaterId { get; set; }

    public DateTime Showtime { get; set; }

    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    [DataType(DataType.Currency)] public decimal TicketPrice { get; set; }

    public Show? Show { get; set; }

    public Theater? Theater { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}