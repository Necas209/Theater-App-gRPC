using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Movement
{
    public Movement()
    {
        Description = "";
    }

    [Key] public int Id { get; set; }

    public int ClientId { get; set; }

    public DateTime Stamp { get; set; } = DateTime.Now;

    [DataType(DataType.Currency)] public decimal Value { get; init; }

    public string Description { get; set; }

    public Client? Client { get; set; }
}