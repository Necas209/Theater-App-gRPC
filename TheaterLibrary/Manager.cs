using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public class Manager
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(TheaterLibrary.User.Manager))]
    public virtual User? User { get; set; }
}