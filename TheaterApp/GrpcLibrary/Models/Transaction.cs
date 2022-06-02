using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Transaction
{
    [Key]
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public DateTime Stamp { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Value { get; set; }

    public string Description { get; set; } = null!;

    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(GrpcLibrary.Models.Client.Transactions))]
    public Client Client { get; set; } = null!;
}