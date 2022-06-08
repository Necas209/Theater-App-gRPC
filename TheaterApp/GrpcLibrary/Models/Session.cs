using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [ForeignKey(nameof(ShowId))]
    [InverseProperty(nameof(Models.Show.Sessions))]
    public Show? Show { get; set; }

    [ForeignKey(nameof(TheaterId))]
    [InverseProperty(nameof(Models.Theater.Sessions))]
    public Theater? Theater { get; set; }

    [InverseProperty(nameof(Reservation.Session))]
    public ICollection<Reservation> Reservations { get; set; }
}