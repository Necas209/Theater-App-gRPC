using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Reservation
{
    [Key] public int Id { get; init; }

    public int ClientId { get; init; }

    public int SessionId { get; init; }

    public int NoTickets { get; init; }

    public DateTime TimeOfPurchase { get; init; }

    [DataType(DataType.Currency)] public decimal Total { get; init; }

    public Client? Client { get; init; }

    public Session? Session { get; init; }
}