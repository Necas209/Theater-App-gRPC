using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Client
{
    [Key] public int Id { get; init; }

    [DataType(DataType.Currency)] public decimal Funds { get; set; }

    [ForeignKey(nameof(Id))] public User? User { get; init; }
}