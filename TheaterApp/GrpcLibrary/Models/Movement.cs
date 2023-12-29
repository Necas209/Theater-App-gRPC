using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Movement
{
    [Key] public int Id { get; init; }

    public int ClientId { get; init; }

    public DateTime Stamp { get; init; } = DateTime.Now;

    [DataType(DataType.Currency)] public decimal Value { get; init; }

    public string Description { get; init; } = string.Empty;

    public Client? Client { get; init; }
}