using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public int SessionId { get; set; }
    
    public int NoTickets { get; set; }
    
    public DateTime TimeOfPurchase { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(TheaterLibrary.Client.Reservations))]
    public virtual Client? Client { get; set; }
    
    [ForeignKey(nameof(SessionId))]
    [InverseProperty(nameof(TheaterLibrary.Session.Reservations))]
    public virtual Session? Session { get; set; }
}