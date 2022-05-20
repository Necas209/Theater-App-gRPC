using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class Transaction
{
    [Key]
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public DateTime Stamp { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Value { get; set; }
    
    public string Description { get; set; }

    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(TheaterLibrary.Client.Transactions))]
    public Client? Client { get; set; }
}