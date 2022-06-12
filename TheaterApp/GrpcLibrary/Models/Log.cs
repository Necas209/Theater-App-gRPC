using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public class Log
{
    [Key] public int Id { get; set; }

    public DateTime Stamp { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(Models.User.Logs))]
    public User? User { get; set; }
}