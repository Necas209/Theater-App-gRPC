using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class Client
{
    public Client()
    {
        Reservations = new HashSet<Reservation>();
        ShowsWatched = new HashSet<Watched>();
        Transactions = new List<Transaction>();
    }

    [Key]
    public int Id { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Funds { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(TheaterLibrary.User.Client))]
    public User? User { get; set; }
    
    [InverseProperty(nameof(Reservation.Client))]
    public ICollection<Reservation> Reservations { get; set; }

    [InverseProperty(nameof(Watched.Client))]
    public ICollection<Watched> ShowsWatched { get; set; }
    
    [InverseProperty(nameof(Transaction.Client))]
    public ICollection<Transaction> Transactions { get; set; }
}