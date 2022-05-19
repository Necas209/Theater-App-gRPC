using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public class Log
{
    [Key]
    public int Id { get; set; }
    
    public DateTime Stamp { get; set; }
    
    public int UserId { get; set; }
    
    public string Message { get; set; }
    
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(TheaterLibrary.User.Logs))]
    public User User { get; set; }
}