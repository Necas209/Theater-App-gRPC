using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Session
{
    [Key] public int Id { get; init; }

    public int ShowId { get; init; }

    public int TheaterId { get; init; }

    public DateTime Showtime { get; set; }

    public int TotalSeats { get; init; }

    public int AvailableSeats { get; set; }

    [DataType(DataType.Currency)] public decimal TicketPrice { get; init; }

    public Show? Show { get; init; }

    public Theater? Theater { get; init; }
}