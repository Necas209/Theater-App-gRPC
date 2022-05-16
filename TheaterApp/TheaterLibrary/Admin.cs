using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public class Admin
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(TheaterLibrary.User.Admin))]
    public virtual User? User { get; set; }
}