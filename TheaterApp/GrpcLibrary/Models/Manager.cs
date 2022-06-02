using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public class Manager
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(GrpcLibrary.Models.User.Manager))]
    public virtual User User { get; set; } = null!;
}