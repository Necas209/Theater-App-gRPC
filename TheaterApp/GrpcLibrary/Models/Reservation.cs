using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public int SessionId { get; set; }
    
    public int NoTickets { get; set; }
    
    public DateTime TimeOfPurchase { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(GrpcLibrary.Models.Client.Reservations))]
    public virtual Client? Client { get; set; }
    
    [ForeignKey(nameof(SessionId))]
    [InverseProperty(nameof(GrpcLibrary.Models.Session.Reservations))]
    public virtual Session? Session { get; set; }
}