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

    [Key]
    public int Id { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Funds { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(Models.User.Client))]
    public User? User { get; set; }

    [InverseProperty(nameof(Reservation.Client))]
    public ICollection<Reservation> Reservations { get; set; }

    [InverseProperty(nameof(Watched.Client))]
    public ICollection<Watched> ShowsWatched { get; set; }
    
    [InverseProperty(nameof(Movement.Client))]
    public ICollection<Movement> Movements { get; set; }
}