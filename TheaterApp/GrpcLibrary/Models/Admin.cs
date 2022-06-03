using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public class Admin
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Id))]
    [InverseProperty(nameof(GrpcLibrary.Models.User.Admin))]
    public virtual User? User { get; set; }
}