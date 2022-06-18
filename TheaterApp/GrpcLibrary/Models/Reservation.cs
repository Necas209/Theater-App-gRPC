using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Reservation
{
    [Key] public int Id { get; set; }

    public int ClientId { get; set; }

    public int SessionId { get; init; }

    public int NoTickets { get; set; }

    public DateTime TimeOfPurchase { get; set; }

    [DataType(DataType.Currency)] public decimal Total { get; set; }

    public Client? Client { get; set; }

    public Session? Session { get; set; }
}