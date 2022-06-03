using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Reservation
{
    [Key]
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public int SessionId { get; set; }
    
    public int NoTickets { get; set; }
    
    public DateTime TimeOfPurchase { get; set; }
    
    public decimal Total { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(Models.Client.Reservations))]
    public Client? Client { get; set; }

    [ForeignKey(nameof(SessionId))]
    [InverseProperty(nameof(Models.Session.Reservations))]
    public Session? Session { get; set; }
}