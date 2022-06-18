using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Client
{
    public Client()
    {
        Reservations = new HashSet<Reservation>();
        ShowsWatched = new HashSet<Watched>();
        Movements = new List<Movement>();
    }

    [Key] public int Id { get; set; }

    [DataType(DataType.Currency)] public decimal Funds { get; set; }

    [ForeignKey(nameof(Id))] public User? User { get; set; }

    public ICollection<Reservation> Reservations { get; set; }

    public ICollection<Watched> ShowsWatched { get; set; }

    public ICollection<Movement> Movements { get; set; }
}